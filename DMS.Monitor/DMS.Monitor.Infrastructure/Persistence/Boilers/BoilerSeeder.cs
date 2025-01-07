using DMS.Monitor.Domain.Boilers;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal static class BoilerSeeder
{
    internal static ApplicationDbContext SeedBoilers(this ApplicationDbContext dbContext)
    {
        if (!dbContext.Boilers.Any())
        {
            var firstBoiler = new Boiler(Guid.NewGuid(), "Boiler #1");
            var secondBoiler = new Boiler(Guid.NewGuid(), "Boiler #2");
            dbContext.Boilers.AddRange(firstBoiler, secondBoiler);
            dbContext.SaveChanges();
        }

        return dbContext;
    }
}
