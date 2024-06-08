using RedditDataRepository.tables.entities;
using System.Linq;
using System.ServiceModel;

namespace RedditDataRepository.tables.interfaces
{
    public interface IAlertEmailRepository
    {
        Task<bool> CreateAsync(AlertEmail alertEmail);

        Task<AlertEmail> ReadAsync(string id);

        Task<IEnumerable<AlertEmail>> ReadAllAsync();

        Task<bool> UpdateAsync(string id, AlertEmail alertEmail);

        Task<bool> DeleteAsync(string id);
    }
}
