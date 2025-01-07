namespace DMS.Monitor.Domain.Base;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent);
}
