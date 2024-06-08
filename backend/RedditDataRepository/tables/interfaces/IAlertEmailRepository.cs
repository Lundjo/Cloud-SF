using RedditDataRepository.tables.entities;
using System.Linq;
using System.ServiceModel;

namespace RedditDataRepository.tables.interfaces
{
    [ServiceContract]
    public interface IAlertEmailRepository
    {
        [OperationContract]
        bool Create(AlertEmail alertEmail);

        [OperationContract]
        AlertEmail Read(string id);

        [OperationContract]
        IQueryable<AlertEmail> ReadAll();

        [OperationContract]
        bool Update(string id, AlertEmail alertEmail);

        [OperationContract]
        bool Delete(string id);
    }
}
