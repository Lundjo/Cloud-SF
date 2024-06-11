using DataRepository.classes.Comments;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.comments.Delete
{
    /// <summary>
    /// Provides methods to delete comments from an Azure Storage Table.
    /// </summary>
    public class RemoveComments
    {
        /// <summary>
        /// Deletes comments associated with the specified post ID from the Azure Storage Table.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="postId">The ID of the post whose comments are to be deleted.</param>
        /// <returns>A Task representing the asynchronous operation. Returns true if the operation was successful; otherwise, false.</returns>
        public static async Task<bool> Execute(CloudTable table, string postId)
        {
            // Check if table is null
            if (table == null)
                return false;

            try
            {
                // Query comments associated with the specified post ID
                TableQuery<Comment> query = new TableQuery<Comment>().Where(TableQuery.GenerateFilterCondition("PostId", QueryComparisons.Equal, postId));
                TableContinuationToken continuationToken = null;
                var commentsToDelete = new List<Comment>();

                // Retrieve comments in batches until no more are available
                do
                {
                    var querySegment = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                    commentsToDelete.AddRange(querySegment.Results);
                    continuationToken = querySegment.ContinuationToken;
                } while (continuationToken != null);

                // Delete each comment
                var deleteTasks = commentsToDelete.Select(comment => table.ExecuteAsync(TableOperation.Delete(comment)));
                var deleteResults = await Task.WhenAll(deleteTasks);

                // Check if all deletions were successful
                return deleteResults.All(result => result.HttpStatusCode == 204);
            }
            catch
            {
                // An error occurred during deletion
                return false;
            }
        }
    }
}
