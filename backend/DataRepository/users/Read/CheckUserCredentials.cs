using DataRepository.classes.Users;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.users.Read
{
    /// <summary>
    /// Provides functionality to check user credentials stored in Azure Table Storage.
    /// </summary>
    public class CheckUserCredentials
    {
        /// <summary>
        /// Checks if the provided email and password match a user record in Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="table">The CloudTable object representing the Azure Table Storage.</param>
        /// <param name="email">The email of the user to be checked.</param>
        /// <param name="password">The password of the user to be checked.</param>
        /// <returns>A Task representing the asynchronous operation that yields a boolean indicating whether the provided email and password match a user record.</returns>
        public static async Task<bool> RunCheck(CloudTable table, string email, string password)
        {
            // Create a TableOperation object to retrieve the entity
            TableOperation retrieveOperation = TableOperation.Retrieve<User>("User", email);

            // Execute the operation asynchronously
            TableResult result = await table.ExecuteAsync(retrieveOperation);

            // Check if the entity exists
            if (result.Result != null)
            {
                // If the user exists, check if the provided password matches
                User user = (User)result.Result;
                return user.Password == password;
            }
            else
            {
                // If the user does not exist, return false
                return false;
            }
        }
    }
}
