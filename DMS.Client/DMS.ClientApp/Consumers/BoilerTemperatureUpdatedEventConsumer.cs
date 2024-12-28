using DMS.Monitor.Contracts.Public.Events.BoilerDevice;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DMS.ClientApp.Consumers;

public class BoilerTemperatureUpdatedEventConsumer : IConsumer<BoilerTemperatureUpdatedEvent>
{
    private readonly ILogger<BoilerTemperatureUpdatedEventConsumer> _logger;

    public BoilerTemperatureUpdatedEventConsumer(ILogger<BoilerTemperatureUpdatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BoilerTemperatureUpdatedEvent> context)
    {
        _logger.LogInformation("Temperature out of range: {Temperature}°C", context.Message.TemperatureCelsius);
    }
}
