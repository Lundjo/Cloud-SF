using RedditDataRepository.Contracts;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace HealthMonitoringService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class HealthMonitoringService : StatelessService
    {
        INotificationServiceContract notification = ServiceProxy.Create<INotificationServiceContract>(new Uri("fabric:/LeRedditService/NotificationService"));

        public HealthMonitoringService(StatelessServiceContext context)
            : base(context)
        { }

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
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    // todo dodaj upis u tabelu
                    bool reddit_online = await HealthCheck();
                    bool not = await notification.IAmAlive();

                    if (reddit_online)
                        ServiceEventSource.Current.ServiceMessage(this.Context, "Reddit is online");
                    else
                        ServiceEventSource.Current.ServiceMessage(this.Context, "Reddit is offline");

                    if (not)
                        ServiceEventSource.Current.ServiceMessage(this.Context, "Notification is online");
                    else
                        ServiceEventSource.Current.ServiceMessage(this.Context, "Notification is offline");

                }
                catch
                {
                    ServiceEventSource.Current.ServiceMessage(this.Context, "Reddit is offline");
                    ServiceEventSource.Current.ServiceMessage(this.Context, "Notification is offline");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        public async Task<bool> HealthCheck()
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync("http://localhost:8370/api/auth/health");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                // Return false if there's an exception
                return false;
            }
        }
    }
}
