using System.Reflection;
using DMS.Monitor.Application.Shared.Extensions;
using DMS.Monitor.Infrastructure.Persistence;
using MassTransit;
using MediatR.NotificationPublishers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Application.Shared.Extensions;

public static class ServiceCollectionExtension
{
    private const string ReadAssemblyName = "DMS.Monitor.Application.Read";
    private const string WriteAssemblyName = "DMS.Monitor.Application.Write";

    public static IServiceCollection AddMassTransit(this IServiceCollection services, string connectionString)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(Assembly.Load(ReadAssemblyName));
            config.AddConsumers(Assembly.Load(WriteAssemblyName));

            config.AddEntityFrameworkOutbox<ApplicationDbContext>(outboxConfig =>
            {
                outboxConfig.UseSqlServer();
                outboxConfig.UseBusOutbox();
            });

            config.AddConfigureEndpointsCallback((context, name, endpointConfig) =>
                endpointConfig.UseEntityFrameworkOutbox<ApplicationDbContext>(context));

            config.UsingRabbitMq((context, config) =>
            {
                config.Host(new Uri(connectionString), "/");

                config.PrefetchCount = 1;
                config.ConfigureEndpoints(context);

                config.UseMessageRetry(r =>
                {
                    var retryCount = 10;
                    r.Immediate(retryCount);
                    r.Handle<DbUpdateException>();
                    r.Handle<SqlException>();
                });

                config.UseInstrumentation();
            });
        });

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                Assembly.Load(ReadAssemblyName),
                Assembly.Load(WriteAssemblyName));

            config.NotificationPublisher = new TaskWhenAllPublisher();
        });

        return services;
    }
}
