using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Users;

namespace RedditDataRepository.users.Read
{
    /// <summary>
    /// Provides methods for reading user profile picture information from Azure Table Storage.
    /// </summary>
    public class ReadUserProfilePicture
    {
        /// <summary>
        /// Retrieves the image blob URL of a user from Azure Table Storage.
        /// </summary>
        /// <param name="table">The CloudTable object representing the table where user data is stored.</param>
        /// <param name="partitionKey">The partition key identifying the user.</param>
        /// <param name="email">The email of the user whose image blob URL is to be retrieved.</param>
        /// <returns>
        /// A string containing the image blob URL of the user.
        /// If the user is not found or an error occurs, an empty string is returned.
        /// </returns>
        public static async Task<string> Run(CloudTable table, string partitionKey, string email)
        {
            try
            {
                // Check if the table is null
                if (table == null)
                    return "";

                // Create a query to retrieve the user based on partition key and email
                var projectionQuery = new TableQuery<User>()
                    .Select(new string[] { "ImageBlobUrl" }) // Select only the imageBlobUrl field
                    .Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email)));

                TableContinuationToken continuationToken = null;
                do
                {
                    // Execute the query segment asynchronously
                    var segment = await table.ExecuteQuerySegmentedAsync(projectionQuery, continuationToken);
                    continuationToken = segment.ContinuationToken;

                    // Iterate through the results and return the imageBlobUrl if found
                    foreach (var entity in segment.Results)
                    {
                        if (entity.RowKey == email)
                        {
                            return entity.ImageBlobUrl;
                        }
                    }
                } while (continuationToken != null);

                // Return an empty string if no matching user is found
                return "";
            }
            catch
            {
                // Return an empty string if an error occurs
                return "";
            }
        }
    }
}