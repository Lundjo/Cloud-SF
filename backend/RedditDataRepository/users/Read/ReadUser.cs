using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Users;

namespace RedditDataRepository.users.Read
{
    /// <summary>
    /// Provides functionality to read user data from Azure Table Storage.
    /// </summary>
    public class ReadUser
    {
        /// <summary>
        /// Retrieves a user from the specified Azure Table Storage table based on the partition key and email.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Table Storage table.</param>
        /// <param name="partitionKey">The partition key of the user to retrieve.</param>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>
        /// The User object representing the retrieved user if found,
        /// or null if no matching user is found or an error occurs.
        /// </returns>
        public static async Task<User> Run(CloudTable table, string partitionKey, string email)
        {
            try
            {
                // Check if the table is null
                if (table == null)
                    return null;

                // Create a query to retrieve the user based on partition key and email
                var query = new TableQuery<User>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email)));

                TableContinuationToken continuationToken = null;
                do
                {
                    // Execute the query segment asynchronously
                    var segment = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = segment.ContinuationToken;

                    // Iterate through the results and return the user if found
                    foreach (var entity in segment.Results)
                    {
                        if (entity.Email == email)
                        {
                            return entity;
                        }
                    }
                } while (continuationToken != null);

                // Return null if no matching user is found
                return null;
            }
            catch
            {
                // Return null if an error occurs
                return null;
            }
        }
    }
}