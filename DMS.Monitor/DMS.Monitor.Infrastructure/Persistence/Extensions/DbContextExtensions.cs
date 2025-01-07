using Microsoft.EntityFrameworkCore;

namespace DMS.Monitor.Infrastructure.Persistence.Extensions;

internal static class DbContextExtensions
{
    internal static TDbContext ApplyMigrations<TDbContext>(this TDbContext dbContext)
        where TDbContext : DbContext
    {
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
        }

        return dbContext;
    }
}
