namespace DMS.Monitor.Domain.Base;

public abstract class EventSourcedAggregateRoot : AggregateRoot
{
    public int Version { get; private set; } = -1;

    public void LoadFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var domainEvent in history)
        {
            Apply(domainEvent);
            Version = domainEvent.Version;
        }
    }

    protected void RaiseDomainEvent<TEvent>(TEvent domainEvent) where TEvent : DomainEvent
    {
        AddDomainEvent(domainEvent);
        Apply(domainEvent);
    }

    protected abstract EventSourcedAggregateRoot Apply(DomainEvent e);
}
