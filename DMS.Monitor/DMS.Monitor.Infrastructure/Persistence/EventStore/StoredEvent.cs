namespace DMS.Monitor.Infrastructure.Persistence.EventStore;

public class StoredEvent
{
    public Guid Id { get; set; }

    public Guid AggregateId { get; set; }

    public required string AggregateType { get; set; }

    public required string EventType { get; set; }

    public required string Data { get; set; }

    public DateTime OccurredOn { get; set; }

    public int Version { get; set; }
}
