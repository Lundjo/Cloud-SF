using DataRepository.classes.Users;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.users.Read
{
    /// <summary>
    /// Provides functionality to check if a user exists in Azure Table Storage.
    /// </summary>
    public class IsUserExists
    {
        /// <summary>
        /// Checks if a user with the provided email exists in Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Table Storage.</param>
        /// <param name="email">The email of the user to check.</param>
        /// <returns>A Task representing the asynchronous operation that yields a boolean indicating whether the user exists.</returns>
        public static async Task<bool> RunCheckAsync(CloudTable table, string email)
        {
            // Create a TableOperation object to retrieve the entity
            TableOperation retrieveOperation = TableOperation.Retrieve<User>("User", email);

            // Execute the operation asynchronously
            TableResult result = await table.ExecuteAsync(retrieveOperation);

            // Check if the entity exists
            return result.Result != null;
        }
    }
}
