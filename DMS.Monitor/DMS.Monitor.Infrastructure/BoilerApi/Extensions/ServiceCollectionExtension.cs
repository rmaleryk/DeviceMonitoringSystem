using DMS.Monitor.Infrastructure.BoilerApi.Client;
using DMS.Monitor.Infrastructure.BoilerApi.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Infrastructure.BoilerApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBoilerApiClient(
        this IServiceCollection services,
        DevicesSettings.BoilerDeviceApiSettings? settings)
    {
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "Boiler device API settings are empty.");
        }

        services.AddHttpClient<IBoilerDeviceApiClient, BoilerDeviceApiClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Host);
        });

        return services;
    }
}
