using DMS.Monitor.Domain.Boilers;

namespace DMS.Monitor.Domain.Persistence;

public interface IBoilerRepository
{
    Task<Boiler> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Boiler>> GetAllAsync(CancellationToken cancellationToken);

    Task SaveAsync(Boiler boiler, CancellationToken cancellationToken);
}
