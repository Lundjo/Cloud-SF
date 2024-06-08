using RedditDataRepository.tables.entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RedditDataRepository.tables.interfaces
{
    public interface IHealthCheckRepository
    {
        Task<bool> CreateAsync(HealthCheck healthCheck);

        Task<HealthCheck> ReadAsync(string dateTime);

        Task<IEnumerable<HealthCheck>> ReadAllAsync();

        Task<bool> UpdateAsync(string dateTime, HealthCheck healthCheck);

        Task<bool> DeleteAsync(string dateTime);

        Task<int> GetCheckCountAsync(DateTimeOffset startDate, DateTimeOffset endDate);

        Task<int> GetOkCheckCountAsync(DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
