using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace RedditDataRepository.cloud.account
{
    /// <summary>
    /// Provides functionality to access Azure Table Storage and retrieve CloudTable objects.
    /// </summary>
    public class AzureTableStorageCloudAccount
    {
        private static CloudStorageAccount _storageAccount;
        private static CloudTableClient _tableClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableStorageCloudAccount"/> class.
        /// </summary>
        public AzureTableStorageCloudAccount()
        {
            _storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            _tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
        }

        /// <summary>
        /// Gets the CloudStorageAccount object representing the Azure Storage account.
        /// </summary>
        /// <returns>The CloudStorageAccount object representing the Azure Storage account.</returns>
        public static CloudStorageAccount GetAccount()
        {
            return _storageAccount;
        }

        /// <summary>
        /// Gets the CloudTable object with the specified table name.
        /// If the table does not exist, it creates the table.
        /// </summary>
        /// <param name="table_name">The name of the table.</param>
        /// <returns>The CloudTable object representing the Azure Table Storage table.</returns>
        public static CloudTable GetCloudTable(string table_name)
        {
            var _table = _tableClient.GetTableReference(table_name);
            _table.CreateIfNotExistsAsync().Wait();

            return _table;
        }
    }
}
