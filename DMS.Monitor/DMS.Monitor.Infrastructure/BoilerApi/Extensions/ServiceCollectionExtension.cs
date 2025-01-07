using DMS.Monitor.Infrastructure.BoilerApi.Client;
using DMS.Monitor.Infrastructure.BoilerApi.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Infrastructure.BoilerApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBoilerApiClient(
        this IServiceCollection services,
        DevicesSettings.BoilerApiSettings? settings)
    {
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "Boiler API settings are empty.");
        }

        services.AddHttpClient<IBoilerApiClient, BoilerApiClient>(client => 
            client.BaseAddress = new Uri(settings.Host));

        return services;
    }
}
