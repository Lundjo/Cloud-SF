using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Comments;

namespace RedditDataRepository.comments.Read
{
    public class ReadComment
    {
        public static async Task<Comment> Run(CloudTable table, string commentId)
        {
            // Construct the TableOperation for retrieve operation.
            TableOperation retrieveOperation = TableOperation.Retrieve<Comment>("Comment", commentId);

            // Execute the retrieve operation.
            TableResult result = await table.ExecuteAsync(retrieveOperation);

            // Check if the operation was successful and return the comment.
            if (result.Result != null)
            {
                return (Comment)result.Result;
            }
            else
            {
                return null;
            }
        }
    }
}
