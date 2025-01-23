using DMS.Monitor.Domain.Boilers;
using DMS.Monitor.Domain.Boilers.Enums;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal static class BoilerSeeder
{
    internal static ApplicationDbContext SeedBoilers(this ApplicationDbContext dbContext)
    {
        if (!dbContext.Boilers.Any())
        {
            var firstBoiler = new Boiler(new Guid("73ac767a-9781-420f-b3d1-2c8724c7a1e3"), "Boiler #1", BoilerState.Off, BoilerTemperature.Default());
            var secondBoiler = new Boiler(new Guid("73ac767a-9781-420f-b3d1-2c8724c7a1e4"), "Boiler #2", BoilerState.Off, BoilerTemperature.Default());
            dbContext.Boilers.AddRange(firstBoiler, secondBoiler);
            dbContext.SaveChanges();
        }

        return dbContext;
    }
}
