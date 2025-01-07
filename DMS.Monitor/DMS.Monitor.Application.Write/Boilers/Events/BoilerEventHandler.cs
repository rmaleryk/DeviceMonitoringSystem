using DMS.Monitor.Contracts.Public.Events.Boilers;
using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DMS.Monitor.Application.Write.Boilers.Events;

public sealed class BoilerEventHandler :
    INotificationHandler<DomainEventNotification<BoilerTurnedOnEvent>>,
    INotificationHandler<DomainEventNotification<BoilerTurnedOffEvent>>,
    INotificationHandler<DomainEventNotification<BoilerTemperatureUpdatedEvent>>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BoilerEventHandler> _logger;

    public BoilerEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<BoilerEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<BoilerTurnedOnEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Boiler with id {Id} has been turned on.", notification.DomainEvent.Id);
        return Task.CompletedTask;
    }

    public Task Handle(DomainEventNotification<BoilerTurnedOffEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Boiler with id {Id} has been turned off.", notification.DomainEvent.Id);
        return Task.CompletedTask;
    }

    public async Task Handle(DomainEventNotification<BoilerTemperatureUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Boiler with id {Id} temperature has been updated.", notification.DomainEvent.Id);

        var domainEvent = notification.DomainEvent;

        await _publishEndpoint.Publish(
            new BoilerTemperatureUpdatedIntegrationEvent(domainEvent.Id, domainEvent.NewTemperature),
            cancellationToken);
    }
}
