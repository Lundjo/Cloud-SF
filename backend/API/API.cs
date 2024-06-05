using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace API
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class API : StatelessService
    {
        public API(StatelessServiceContext context)
            : base(context)
        { }


        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);


                        _ = builder.Services.AddCors();
                        _ = builder.Services.AddControllers();
                        _ = builder.Services.AddDistributedMemoryCache();
                        _ = builder.Services.AddSession(options =>
                        {
                            options.IdleTimeout = TimeSpan.FromMinutes(60);
                            options.Cookie.HttpOnly = true;
                            options.Cookie.IsEssential = true;
                        });

                        WebApplication app = builder.Build();

                        _ = app.UseSession();

                        _ = app.UseCors(options =>
                        {
                            _ = options.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                        });

                        _ = app.MapControllers();

                        return app;

                    }))
            };
        }
    }
}
