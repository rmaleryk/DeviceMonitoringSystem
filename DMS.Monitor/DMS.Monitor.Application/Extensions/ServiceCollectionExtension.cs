using System.Reflection;
using DMS.Monitor.Application.Configuration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(Assembly.Load("DMS.Monitor.Application"));

            config.UsingRabbitMq((context, config) =>
            {
                config.Host(rabbitMqSettings.Host, "/", h =>
                {
                    h.Username(rabbitMqSettings.Username!);
                    h.Password(rabbitMqSettings.Password!);
                });

                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
