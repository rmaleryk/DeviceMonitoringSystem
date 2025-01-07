using DMS.Monitor.Infrastructure.Persistence.Boilers;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Monitor.Infrastructure.Persistence.Extensions;

public static class ServiceScopeExtensions
{
    public static IServiceScope MigrateDatabase(this IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.ApplyMigrations();

        return scope;
    }

    public static IServiceScope SeedDatabase(this IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.SeedBoilers();

        return scope;
    }
}