using RedditDataRepository.Contracts;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using Microsoft.WindowsAzure.Storage.Queue;
using RedditDataRepository.tables;
using RedditDataRepository.cloud.queue;
using RedditDataRepository.classes.Logs;
using RedditDataRepository.cloud.account;
using RedditDataRepository.comments.Read;
using RedditDataRepository.logs.Create;
using RedditDataRepository.queues;
using RedditDataRepository;

namespace NotificationService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class NotificationService : StatelessService, INotificationServiceContract
    {
        private CloudQueue queue;
        private CloudQueue adminQueue;
        private AlertEmailRepository adminEmailRepo;

        public NotificationService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<bool> IAmAlive()
        {
            await Task.Delay(1000);
            return true;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            queue = AzureQueueHelper.GetQueue("notifications");
            adminQueue = AzureQueueHelper.GetQueue("adminnotificationqueue");
            adminEmailRepo = new AlertEmailRepository();

            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessQueueMessagesAsync(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        private async Task ProcessQueueMessagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                string adminMessage = await NotificationQueue.DequeueComment(adminQueue);
                string commentId = await NotificationQueue.DequeueComment(queue);

                if (commentId == null && adminMessage == null)
                {
                    return;
                }

                if (commentId != null)
                {
                    await ProcessCommentAsync(commentId);
                }

                if (adminMessage != null)
                {
                    await ProcessAdminMessageAsync(adminMessage);
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(Context, $"Exception: {ex.Message}");
            }
        }

        private async Task ProcessCommentAsync(string commentId)
        {
            List<string> emails = await CommentService.GetPostEmails(commentId);
            string commentText = (await ReadComment.Run(AzureTableStorageCloudAccount.GetCloudTable("comments"), commentId)).Content;
            int numOfEmailsSent = 0;

            foreach (string email in emails)
            {
                if (await CommentService.SendEmail(email, commentText))
                {
                    ++numOfEmailsSent;
                }
            }

            if (!(await InsertEmailLog.Execute(AzureTableStorageCloudAccount.GetCloudTable("emailLogs"), new EmailLog(DateTime.Now, commentId, numOfEmailsSent))))
            {
                ServiceEventSource.Current.ServiceMessage(Context, "Error inserting email log into table.");
            }
        }

        private async Task ProcessAdminMessageAsync(string adminMessage)
        {
            List<AlertEmailDTO> alertEmailDTOs = (await adminEmailRepo.ReadAllAsync()).Select(alertEmail => new AlertEmailDTO(alertEmail.RowKey, alertEmail.Email)).ToList();

            if (alertEmailDTOs.Count == 0)
            {
                return;
            }

            foreach (AlertEmailDTO dto in alertEmailDTOs)
            {
                await CommentService.SendEmail(dto.Email, adminMessage);
            }
        }
    }
}
