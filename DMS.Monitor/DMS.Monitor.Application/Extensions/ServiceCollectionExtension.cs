using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, string connectionString)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(Assembly.GetExecutingAssembly());

            config.UsingRabbitMq((context, config) =>
            {
                config.Host(new Uri(connectionString), "/");

                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
