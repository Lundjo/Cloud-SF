using Microsoft.WindowsAzure.Storage.Queue;
using RedditDataRepository.classes.Comments;

namespace RedditDataRepository.queues
{
    public class NotificationQueue
    {
        public static void EnqueueComment(CloudQueue queue, Comment comment)
        {
            CloudQueueMessage message = new CloudQueueMessage(comment.Id);
            queue.AddMessageAsync(message);
        }

        public async static Task<string> DequeueComment(CloudQueue queue)
        {
            CloudQueueMessage message = await queue.GetMessageAsync();
            if (message != null)
            {
                await queue.DeleteIfExistsAsync();
                return message.AsString;
            }
            return null;
        }
    }
}
