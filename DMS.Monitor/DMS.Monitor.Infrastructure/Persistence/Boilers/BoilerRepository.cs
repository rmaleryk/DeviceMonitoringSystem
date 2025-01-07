using DMS.Monitor.Domain.Boilers;
using DMS.Monitor.Domain.Persistence;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal sealed class BoilerRepository : GenericRepository<Boiler>, IBoilerRepository
{
    public BoilerRepository(ApplicationDbContext context) 
        : base(context)
    {
    }
}
