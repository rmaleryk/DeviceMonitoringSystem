namespace DMS.Monitor.Contracts.Public.Events.Boilers;

public sealed record class BoilerTemperatureUpdatedEvent(Guid Id, double TemperatureCelsius);
