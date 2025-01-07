using DMS.Monitor.Domain.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DMS.Monitor.Infrastructure.DomainEvents;

public class MediatorDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<MediatorDomainEventDispatcher> _log;

    public MediatorDomainEventDispatcher(
        IMediator mediator,
        ILogger<MediatorDomainEventDispatcher> log)
    {
        _mediator = mediator;
        _log = log;
    }

    public async Task Dispatch(DomainEvent domainEvent)
    {
        INotification notification = CreateDomainEventNotification(domainEvent);
        _log.LogDebug("Dispatching Domain Event as MediatR notification. EventType: {EventType}", domainEvent.GetType());
        await _mediator.Publish(notification);
    }

    private static INotification CreateDomainEventNotification(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}
