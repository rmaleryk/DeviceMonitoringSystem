using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Persistence;
using DMS.Monitor.Infrastructure.DomainEvents;
using DMS.Monitor.Infrastructure.Persistence.Boilers;
using DMS.Monitor.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Database connection string is empty", nameof(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.UseSqlServer(
                connectionString,
                opts => opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });

        services.AddScoped(_ => new SqlInitializationData(connectionString));
        services.AddTransient<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

        return services
            .AddWriteLayer()
            .AddReadLayer();
    }

    public static IServiceCollection AddReadLayer(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddWriteLayer(this IServiceCollection services)
    {
        services
            .AddTransient<IBoilerRepository, BoilerRepository>();

        return services;
    }
}
