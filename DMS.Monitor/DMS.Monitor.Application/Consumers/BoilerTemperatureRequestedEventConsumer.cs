using DMS.Monitor.Application.Configuration;
using DMS.Monitor.Contracts.Public.Events.BoilerDevice;
using DMS.Monitor.Domain.Converters;
using DMS.Monitor.Infrastructure.BoilerApi.Client;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DMS.Monitor.Application.Consumers;

public class BoilerTemperatureRequestedEventConsumer : IConsumer<BoilerTemperatureRequestedEvent>
{
    private readonly IBus _bus;
    private readonly IBoilerDeviceApiClient _boilerDeviceApiClient;
    private readonly ITemperatureConverter _temperatureConverter;
    private readonly ILogger<BoilerTemperatureRequestedEventConsumer> _logger;
    private readonly TemperatureThresholds _temperatureThresholds;

    public BoilerTemperatureRequestedEventConsumer(
        IBus bus,
        IBoilerDeviceApiClient boilerApiClient,
        ITemperatureConverter temperatureConverter,
        ILogger<BoilerTemperatureRequestedEventConsumer> logger,
        IOptions<TemperatureThresholds> temperatureThresholds)
    {
        _bus = bus;
        _boilerDeviceApiClient = boilerApiClient;
        _temperatureConverter = temperatureConverter;
        _logger = logger;
        _temperatureThresholds = temperatureThresholds.Value;
    }

    public async Task Consume(ConsumeContext<BoilerTemperatureRequestedEvent> context)
    {
        var temperatureFahrenheit = await _boilerDeviceApiClient.GetTemperatureAsync();
        var temperatureCelsius = _temperatureConverter.ToCelsius(temperatureFahrenheit);

        ValidateTemperature(temperatureCelsius);

        await _bus.Publish(new BoilerTemperatureUpdatedEvent(temperatureCelsius));
    }

    private void ValidateTemperature(double temperatureCelsius)
    {
        if (temperatureCelsius < _temperatureThresholds.MinTemperature ||
            temperatureCelsius > _temperatureThresholds.MaxTemperature)
        {
            _logger.LogError("Temperature out of range: {Temperature}°C", temperatureCelsius);
        }
    }
}
