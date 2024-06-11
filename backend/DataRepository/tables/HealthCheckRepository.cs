using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using DataRepository.tables.entities;
using System;
using System.Linq;

namespace DataRepository.tables
{
    public class HealthCheckRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        #region Init
        public HealthCheckRepository()
        {
            //_storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            _storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("HealthCheckTable");
            _table.CreateIfNotExistsAsync().Wait();
        }
        #endregion

        #region CRUD Operations
        public async Task<bool> CreateAsync(HealthCheck healthCheck)
        {
            if (healthCheck == null)
                return false;

            try
            {
                TableOperation insertOperation = TableOperation.Insert(healthCheck);
                await _table.ExecuteAsync(insertOperation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<HealthCheck> ReadAsync(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
                return new HealthCheck();

            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<HealthCheck>("HealthCheck", dateTime);
                TableResult result = await _table.ExecuteAsync(retrieveOperation);
                return result.Result as HealthCheck ?? new HealthCheck();
            }
            catch (Exception)
            {
                return new HealthCheck();
            }
        }

        public async Task<IEnumerable<HealthCheck>> ReadAllAsync()
        {
            try
            {
                var query = new TableQuery<HealthCheck>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "HealthCheck"));

                var results = new List<HealthCheck>();
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
                return Enumerable.Empty<HealthCheck>().AsQueryable();
            }
        }

        public async Task<bool> UpdateAsync(string dateTime, HealthCheck healthCheck)
        {
            if (string.IsNullOrEmpty(dateTime) || healthCheck == null)
                return false;

            try
            {
                var existingHealthCheck = await ReadAsync(dateTime);

                if (existingHealthCheck != null)
                {
                    existingHealthCheck.Status = healthCheck.Status;
                    existingHealthCheck.Service = healthCheck.Service;

                    TableOperation updateOperation = TableOperation.Replace(existingHealthCheck);

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

        public async Task<bool> DeleteAsync(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
                return false;

            var healthCheckToDelete = await ReadAsync(dateTime);

            if (healthCheckToDelete != null)
            {
                try
                {
                    TableOperation deleteOperation = TableOperation.Delete(healthCheckToDelete);

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

        public async Task<int> GetCheckCountAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string startRowKey = startDate.ToString("yyyyMMddHHmmss") + startDate.Millisecond.ToString("000");
            string endRowKey = endDate.ToString("yyyyMMddHHmmss") + endDate.Millisecond.ToString("000");

            var query = new TableQuery<HealthCheck>()
                .Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "HealthCheck")
                    + " and " +
                    TableQuery.GenerateFilterCondition("Service", QueryComparisons.Equal, "Reddit")
                    + " and " +
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey)
                    + " and " +
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, endRowKey)
                );

            var results = new List<HealthCheck>();
            TableContinuationToken token = null;

            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                results.AddRange(segment.Results);
            } while (token != null);

            return results.Count;
        }

        public async Task<int> GetOkCheckCountAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string startRowKey = startDate.ToString("yyyyMMddHHmmss") + startDate.Millisecond.ToString("000");
            string endRowKey = endDate.ToString("yyyyMMddHHmmss") + endDate.Millisecond.ToString("000");

            var query = new TableQuery<HealthCheck>()
                .Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "HealthCheck")
                    + " and " +
                    TableQuery.GenerateFilterCondition("Status", QueryComparisons.Equal, "OK")
                    + " and " +
                    TableQuery.GenerateFilterCondition("Service", QueryComparisons.Equal, "Reddit")
                    + " and " +
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey)
                    + " and " +
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, endRowKey)
                );

            var results = new List<HealthCheck>();
            TableContinuationToken token = null;

            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                results.AddRange(segment.Results);
            } while (token != null);

            return results.Count;
        }
        #endregion
    }
}
