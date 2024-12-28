using System.Reflection;
using DMS.ClientApp.Configuration;
using DMS.ClientApp.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(Assembly.GetEntryAssembly());

            config.UsingRabbitMq((context, config) =>
            {
                var rabbitMqSettings = hostContext.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>()!;

                config.Host(rabbitMqSettings.Host, "/", h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddHostedService<TemperatureRequestService>();
    })
    .Build()
    .RunAsync();