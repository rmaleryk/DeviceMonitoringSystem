namespace DMS.Monitor.Domain.Base;

public abstract record DomainEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();

    public DateTime EventDate { get; set; } = DateTime.UtcNow;

}
