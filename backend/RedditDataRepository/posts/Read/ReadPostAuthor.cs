using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Posts;

namespace RedditDataRepository.posts.Read
{
    /// <summary>
    /// Provides methods to read a post author by its ID from an Azure Storage Table.
    /// </summary>
    public class ReadPostAuthor
    {
        /// <summary>
        /// Reads the author of a single post from the specified Azure Storage Table by its ID.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Storage Table.</param>
        /// <param name="postId">The ID of the post to retrieve.</param>
        /// <returns>A Task that, when completed, returns the author (string) of the retrieved post.</returns>
        public static async Task<string> Execute(CloudTable table, string postId)
        {
            try
            {
                // Check if the table and postId are not null or empty
                if (table == null || string.IsNullOrEmpty(postId))
                    return null;

                // Create a TableQuery to project only the Author property
                var query = new TableQuery<Post>().Select(new string[] { "Author" }).Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Post"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, postId)));

                // Execute the query asynchronously
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);

                // Get the first post from the results (should only be one)
                var post = queryResult?.Results?.Count > 0 ? queryResult.Results[0] : null;

                // Return the author of the post
                return post?.Author;
            }
            catch
            {
                return null;
            }
        }
    }
}
