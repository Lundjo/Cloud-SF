using Microsoft.ServiceFabric.Services.Remoting;

namespace RedditDataRepository.Contracts
{
    public interface INotificationServiceContract : IService
    {
        Task<bool> IAmAlive();
    }
}
