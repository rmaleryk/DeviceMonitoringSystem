using DMS.Monitor.Application.Configuration;
using DMS.Monitor.Contracts.Public.Events.BoilerDevice;
using DMS.Monitor.Domain.Converters;
using DMS.Monitor.Infrastructure.BoilerApi.Client;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DMS.Monitor.Application.Consumers;

public class BoilerTemperatureRequestedEventConsumer(
    IBus bus,
    IBoilerDeviceApiClient boilerApiClient,
    ITemperatureConverter temperatureConverter,
    ILogger<BoilerTemperatureRequestedEventConsumer> logger,
    IOptions<TemperatureThresholds> temperatureThresholds) : IConsumer<BoilerTemperatureRequestedEvent>
{
    public async Task Consume(ConsumeContext<BoilerTemperatureRequestedEvent> context)
    {
        var temperatureFahrenheit = await boilerApiClient.GetTemperatureAsync();
        var temperatureCelsius = temperatureConverter.ToCelsius(temperatureFahrenheit);

        ValidateTemperature(temperatureCelsius);

        await bus.Publish(new BoilerTemperatureUpdatedEvent(temperatureCelsius));
    }

    private void ValidateTemperature(double temperatureCelsius)
    {
        if (temperatureCelsius < temperatureThresholds.Value.MinTemperature ||
            temperatureCelsius > temperatureThresholds.Value.MaxTemperature)
        {
            logger.LogError("Temperature out of range: {Temperature}°C", temperatureCelsius);
        }
    }
}
