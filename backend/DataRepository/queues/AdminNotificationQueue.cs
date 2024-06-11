using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;

namespace DataRepository.queues
{
    public class AdminNotificationQueue
    {
        public static CloudQueue GetQueue(string queueName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference(queueName);
                queue.CreateIfNotExistsAsync().Wait();
                return queue;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return null;
        }

        public static void EnqueueMessage(CloudQueue queue, string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            queue.AddMessageAsync(queueMessage);
        }

        public async static Task<string> DequeueMessage(CloudQueue queue)
        {
            CloudQueueMessage queueMessage = await queue.GetMessageAsync();
            if (queueMessage != null)
            {
                return queueMessage.AsString;
            }
            return null;
        }
    }
}
