using DataRepository.classes.Users;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataRepository.users.Read
{
    /// <summary>
    /// Provides methods for reading user profile picture information from Azure Table Storage.
    /// </summary>
    public class ReadUserProfilePicture
    {

        public static async Task<string> Run(CloudTable table, string rowKey)
        {
            try
            {
                if (table == null || string.IsNullOrEmpty(rowKey))
                    return null;

                TableOperation retrieveOperation = TableOperation.Retrieve<User>("User", rowKey);
                return ((await table.ExecuteAsync(retrieveOperation)).Result as User).ImageBlobUrl;
            }
            catch
            {
                return "";
            }
        }
    }
}