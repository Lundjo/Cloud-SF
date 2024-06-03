using Microsoft.WindowsAzure.Storage.Table;
using RedditDataRepository.classes.Users;
using RedditDataRepository.users.Read;

namespace RedditDataRepository.users.Update
{
    /// <summary>
    /// Provides functionality to update a user entity in Azure Table Storage.
    /// </summary>
    public class UpdateUser
    {
        /// <summary>
        /// Executes the update operation for a user entity in the specified CloudTable.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Table where the user entity is stored.</param>
        /// <param name="user">The User object representing the user entity to be updated.</param>
        /// <returns>A Task object representing the asynchronous operation with a boolean value indicating whether the update was successful.</returns>
        public static async Task<bool> Execute(CloudTable table, User user)
        {
            // Check if user or table is null
            if (user == null || table == null)
                return false;

            // Check if user doesn't exist
            if (!await IsUserExists.RunCheckAsync(table, user.Email))
                return false;

            try
            {
                // Create a TableOperation object to insert or replace the entity
                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(user);

                // Execute the operation asynchronously
                TableResult result = await table.ExecuteAsync(insertOrReplaceOperation);

                // Check if the operation was successful
                return result.HttpStatusCode == 200 || result.HttpStatusCode == 204;
            }
            catch
            {
                // An error occurred during insertion
                return false;
            }
        }
    }
}
