namespace DMS.Monitor.Contracts.Public.Events.Boilers;

public sealed record class BoilerTemperatureUpdatedIntegrationEvent(Guid Id, double TemperatureCelsius);
