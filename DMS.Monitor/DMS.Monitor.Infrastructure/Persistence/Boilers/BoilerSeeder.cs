using DMS.Monitor.Domain.Boilers;
using DMS.Monitor.Domain.Boilers.Enums;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal static class BoilerSeeder
{
    internal static ApplicationDbContext SeedBoilers(this ApplicationDbContext dbContext)
    {
        if (!dbContext.Boilers.Any())
        {
            var firstBoiler = new Boiler(Guid.NewGuid(), "Boiler #1", BoilerState.Off, BoilerTemperature.Default());
            var secondBoiler = new Boiler(Guid.NewGuid(), "Boiler #2", BoilerState.Off, BoilerTemperature.Default());
            dbContext.Boilers.AddRange(firstBoiler, secondBoiler);
            dbContext.SaveChanges();
        }

        return dbContext;
    }
}
