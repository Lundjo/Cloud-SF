using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Comments;

namespace RedditDataRepository.comments.Read
{
    /// <summary>
    /// Provides methods to read comments from an Azure Storage Table.
    /// </summary>
    public class ReadComments
    {
        /// <summary>
        /// Reads a collection of comments from the specified Azure Storage Table by the post ID.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="postId">The ID of the post for which to retrieve the comments.</param>
        /// <returns>A Task that, when completed, returns a List of Comment objects representing the retrieved comments.</returns>
        public static async Task<List<Comment>> Execute(CloudTable table, string postId)
        {
            // Create a TableQuery object to retrieve all comments for the specified post ID
            var query = new TableQuery<Comment>().Where(TableQuery.CombineFilters(
                                                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Comment"),
                                                        TableOperators.And,
                                                        TableQuery.GenerateFilterCondition("PostId", QueryComparisons.Equal, postId)));

            // Execute the query and retrieve the comments
            var comments = new List<Comment>();
            TableContinuationToken continuationToken = null;
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                comments.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;
            } while (continuationToken != null);

            return comments;
        }
    }
}