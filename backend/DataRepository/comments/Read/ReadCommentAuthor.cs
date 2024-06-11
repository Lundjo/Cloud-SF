using DataRepository.classes.Comments;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.comments.Read
{
    /// <summary>
    /// Provides methods to read a comment author by its ID from an Azure Storage Table.
    /// </summary>
    public class ReadCommentAuthor
    {
        /// <summary>
        /// Reads the author of a single comment from the specified Azure Storage Table by its ID.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="commentId">The ID of the comment to retrieve.</param>
        /// <returns>A Task that, when completed, returns the author (string) of the retrieved comment.</returns>
        public static async Task<string> Execute(CloudTable table, string commentId)
        {
            try
            {
                // Check if the table and commentId are not null or empty
                if (table == null || string.IsNullOrEmpty(commentId))
                    return null;

                // Create a TableQuery to project only the Author property
                var query = new TableQuery<Comment>().Select(new string[] { "Author" }).Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Comment"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, commentId)));

                // Execute the query asynchronously
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);

                // Get the first comment from the results (should only be one)
                var comment = queryResult?.Results?.Count > 0 ? queryResult.Results[0] : null;

                // Return the author of the comment
                return comment?.Author;
            }
            catch
            {
                return null;
            }
        }
    }
}
