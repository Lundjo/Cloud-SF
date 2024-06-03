using Microsoft.WindowsAzure.Storage.Queue;
using RedditDataRepository.cloud.account;
using System.Diagnostics;

namespace RedditDataRepository.cloud.queue
{
    public class AzureQueueHelper
    {
        public static CloudQueue GetQueue(string queueName)
        {
            try
            {
                /*CloudStorageAccount account = AzureTableStorageCloudAccount.GetAccount();
                if(account == null)
                {
                    AzureTableStorageCloudAccount account1 = new AzureTableStorageCloudAccount();
                }*/
                CloudQueueClient client = AzureTableStorageCloudAccount.GetAccount().CreateCloudQueueClient();
                CloudQueue queue = client.GetQueueReference(queueName);
                queue.CreateIfNotExistsAsync().Wait();
                return queue;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return null;
        }
    }
}
