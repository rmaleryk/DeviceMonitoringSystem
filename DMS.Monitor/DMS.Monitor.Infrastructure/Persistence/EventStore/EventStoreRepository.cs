using System.Text.Json;
using DMS.Monitor.Domain.Base;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DMS.Monitor.Infrastructure.Persistence.EventStore;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly JsonSerializerOptions _serializerOptions;

    public EventStoreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
    }

    public async Task SaveEventsAsync<TAggregate>(
        Guid aggregateId,
        IEnumerable<DomainEvent> events,
        int expectedVersion,
        CancellationToken cancellationToken) where TAggregate : EventSourcedAggregateRoot
    {
        var lastEvent = await _dbContext.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderByDescending(e => e.Version)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastEvent != null && expectedVersion == lastEvent.Version)
        {
            throw new ConcurrencyException();
        }

        var version = expectedVersion;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;

            var storedEvent = new StoredEvent
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregateId,
                AggregateType = typeof(TAggregate).Name,
                EventType = @event.GetType().Name,
                Data = JsonSerializer.Serialize(@event, _serializerOptions),
                OccurredOn = @event.OccurredOn,
                Version = version
            };

            _dbContext.StoredEvents.Add(storedEvent);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<DomainEvent>> GetEventsAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var storedEvents = await _dbContext.StoredEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        var domainEvents = storedEvents
            .Select(e =>
            {
                var eventType = Type.GetType(e.EventType) 
                    ?? throw new InvalidDataException();

                return JsonSerializer.Deserialize(e.Data, eventType, _serializerOptions) as DomainEvent;
            })
            .ToList();

        return domainEvents!;
    }

    public async Task<List<Guid>> GetAllAggregateIdsAsync<TAggregate>(CancellationToken cancellationToken)
        where TAggregate : EventSourcedAggregateRoot
    {
        var aggregateType = typeof(TAggregate).Name;
        return await _dbContext.StoredEvents
            .Where(e => e.AggregateType == aggregateType)
            .Select(e => e.AggregateId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
