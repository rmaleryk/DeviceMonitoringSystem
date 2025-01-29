namespace DMS.Monitor.Domain.Base;

public abstract record DomainEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();

    public DateTime OccurredOn { get; protected set; }

    public int Version { get; set; }
}
