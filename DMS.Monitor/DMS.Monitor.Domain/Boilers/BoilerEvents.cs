using DMS.Monitor.Domain.Base;

namespace DMS.Monitor.Domain.Boilers;

public record BoilerTurnedOnEvent(Guid Id) : DomainEvent;

public record BoilerTurnedOffEvent(Guid Id) : DomainEvent;

public record BoilerTemperatureUpdatedEvent(Guid Id, double NewTemperature) : DomainEvent;
