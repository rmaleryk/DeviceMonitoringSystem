using BoilerMonitor.Infrastructure.BoilerApi.Client;
using BoilerMonitor.Infrastructure.BoilerApi.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerMonitor.Infrastructure.BoilerApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBoilerApiClient(this IServiceCollection services, BoilerApiSettings settings)
        {
            services.AddHttpClient<IBoilerApiClient, BoilerApiClient>(client =>
            {
                client.BaseAddress = new Uri(settings.Host);
            });

            return services;
        }
    }
}
