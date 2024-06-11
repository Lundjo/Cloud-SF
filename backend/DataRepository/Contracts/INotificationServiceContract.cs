using Microsoft.ServiceFabric.Services.Remoting;

namespace DataRepository.Contracts
{
    public interface INotificationServiceContract : IService
    {
        Task<bool> IAmAlive();
    }
}
