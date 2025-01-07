using DMS.Client.Api.Caching;
using DMS.Monitor.Contracts.Public.Events.Boilers;
using MassTransit;

namespace DMS.Client.Api.Consumers;

public class BoilerTemperatureUpdatedEventConsumer(
    BoilerTemperatureCache boilerTemperatureCache,
    ILogger<BoilerTemperatureUpdatedEventConsumer> logger) : IConsumer<BoilerTemperatureUpdatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<BoilerTemperatureUpdatedIntegrationEvent> context)
    {
        logger.LogInformation("Saving updated temperature: {Temperature}°C", context.Message.TemperatureCelsius);

        boilerTemperatureCache.SetTemperature(context.Message.Id, context.Message.TemperatureCelsius);

        return Task.CompletedTask;
    }
}
