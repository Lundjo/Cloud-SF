using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using DataRepository.tables.entities;
using System;
using System.Linq;

namespace DataRepository.tables
{
    public class AlertEmailRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        #region Init
        public AlertEmailRepository()
        {
            _storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudTableClient tableClient = new CloudTableClient(new
            Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("AlertEmailTable");
            _table.CreateIfNotExistsAsync().Wait();
        }
        #endregion

        #region CRUD Operations
        public async Task<bool> CreateAsync(AlertEmail alertEmail)
        {
            if (alertEmail == null)
                return false;

            try
            {
                TableOperation insertOperation = TableOperation.Insert(alertEmail);
                await _table.ExecuteAsync(insertOperation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AlertEmail> ReadAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new AlertEmail();

            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<HealthCheck>("HealthCheck", id);
                TableResult result = await _table.ExecuteAsync(retrieveOperation);
                return result.Result as AlertEmail ?? new AlertEmail();
            }
            catch (Exception)
            {
                return new AlertEmail();
            }
        }

        public async Task<IEnumerable<AlertEmail>> ReadAllAsync()
        {
            try
            {
                var query = new TableQuery<AlertEmail>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "AlertEmail"));

                var results = new List<AlertEmail>();
                TableContinuationToken token = null;

                do
                {
                    var segment = await _table.ExecuteQuerySegmentedAsync(query, token);
                    token = segment.ContinuationToken;
                    results.AddRange(segment.Results);
                } while (token != null);
                return results;
            }
            catch (Exception)
            {
                return Enumerable.Empty<AlertEmail>().AsQueryable();
            }
        }

        public async Task<bool> UpdateAsync(string id, AlertEmail alertEmail)
        {
            if (string.IsNullOrEmpty(id) || alertEmail == null)
                return false;

            try
            {
                var existingAlertEmailCheck = await ReadAsync(id);

                if (existingAlertEmailCheck != null)
                {
                    existingAlertEmailCheck.Email = alertEmail.Email;

                    TableOperation updateOperation = TableOperation.Replace(existingAlertEmailCheck);

                    await _table.ExecuteAsync(updateOperation);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            var alertEmailCheckToDelete = await ReadAsync(id);

            if (alertEmailCheckToDelete != null)
            {
                try
                {
                    TableOperation deleteOperation = TableOperation.Delete(alertEmailCheckToDelete);

                    await _table.ExecuteAsync(deleteOperation);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
