using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers;
using DMS.Monitor.Domain.Persistence;
using DMS.Monitor.Infrastructure.Persistence.EventStore;

namespace DMS.Monitor.Infrastructure.Persistence.Boilers;

internal sealed class BoilerRepository : IBoilerRepository
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public BoilerRepository(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task<Boiler> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var events = await _eventStoreRepository.GetEventsAsync(id, cancellationToken);
        var boiler = ReplayEntity(id, events);
        return boiler;
    }

    public async Task<List<Boiler>> GetAllAsync(CancellationToken cancellationToken)
    {
        var boilerIds = await _eventStoreRepository.GetAllAggregateIdsAsync<Boiler>(cancellationToken);
        var boilers = new List<Boiler>();

        foreach (var id in boilerIds)
        {
            var events = await _eventStoreRepository.GetEventsAsync(id, cancellationToken);
            var boiler = ReplayEntity(id, events);
            boilers.Add(boiler);
        }

        return boilers;
    }

    public async Task SaveAsync(Boiler boiler, CancellationToken cancellationToken)
    {
        await _eventStoreRepository.SaveEventsAsync<Boiler>(
            boiler.Id,
            boiler.DomainEvents,
            boiler.Version,
            cancellationToken);
    }

    private static Boiler ReplayEntity(Guid id, List<DomainEvent> events)
    {
        var boiler = Activator.CreateInstance<Boiler>();
        boiler.Id = id;
        boiler.LoadFromHistory(events);
        return boiler;
    }
}
