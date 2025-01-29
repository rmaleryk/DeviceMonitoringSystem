using DMS.Monitor.Domain.Base;

namespace DMS.Monitor.Infrastructure.Persistence.EventStore;

public interface IEventStoreRepository
{
    Task<List<DomainEvent>> GetEventsAsync(Guid aggregateId, CancellationToken cancellationToken);

    Task<List<Guid>> GetAllAggregateIdsAsync<TAggregate>(CancellationToken cancellationToken)
        where TAggregate : EventSourcedAggregateRoot;

    Task SaveEventsAsync<TAggregate>(
        Guid aggregateId,
        IEnumerable<DomainEvent> events,
        int expectedVersion,
        CancellationToken cancellationToken) where TAggregate : EventSourcedAggregateRoot;
}
