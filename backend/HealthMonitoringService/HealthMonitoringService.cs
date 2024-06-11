using System;
using System.Collections.Generic;
using System.Fabric;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataRepository.Contracts;
using DataRepository.tables;
using DataRepository.tables.entities;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace HealthMonitoringService
{
    internal sealed class HealthMonitoringService : StatelessService
    {
        private static INotificationServiceContract _notificationService;
        private static HealthCheckRepository Repository;

        public HealthMonitoringService(StatelessServiceContext context)
            : base(context)
        {
            try
            {
                Repository = new HealthCheckRepository();
                ServiceEventSource.Current.ServiceMessage(context, "HealthCheckRepository instantiated successfully.");
                // Initialize the notification service proxy
                _notificationService = ServiceProxy.Create<INotificationServiceContract>(
                    new Uri("fabric:/LeRedditService/NotificationService"));
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(context, $"Failed to instantiate HealthCheckRepository: {ex.Message}");
            }
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    bool redditOnline = await HealthCheck("http://localhost:8370/api/auth/health");
                    bool notificationOnline = await _notificationService.IAmAlive();

                    LogHealthCheck(redditOnline, "Reddit");
                    LogHealthCheck(notificationOnline, "Notification");
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.ServiceMessage(this.Context, $"Error in Health Monitoring: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        private async Task<bool> HealthCheck(string url)
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                // Return false if there's an exception
                return false;
            }
        }

        private void LogHealthCheck(bool isOnline, string serviceName)
        {
            string status = isOnline ? "Online" : "Offline";
            DateTime timestamp = DateTime.UtcNow;
            HealthCheck healthCheckEntity = new HealthCheck(timestamp.ToString("yyyyMMddHHmmssfff"), status, serviceName);
            Repository.CreateAsync(healthCheckEntity).Wait();
        }
    }
}
