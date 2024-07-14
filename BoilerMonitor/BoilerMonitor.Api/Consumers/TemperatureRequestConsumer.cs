using BoilerMonitor.Api.Configuration;
using BoilerMonitor.Domain.Converters;
using BoilerMonitor.Infrastructure.BoilerApi.Client;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedKernel.Contracts;

namespace BoilerMonitor.Api.Consumers
{
    public class TemperatureRequestConsumer : IConsumer<TemperatureRequest>
    {
        private readonly IBoilerApiClient _boilerApiClient;
        private readonly ITemperatureConverter _temperatureConverter;
        private readonly ILogger<TemperatureRequestConsumer> _logger;
        private readonly TemperatureThresholds _temperatureThresholds;

        public TemperatureRequestConsumer(
            IBoilerApiClient boilerApiClient,
            ITemperatureConverter temperatureConverter,
            ILogger<TemperatureRequestConsumer> logger,
            IOptions<TemperatureThresholds> temperatureThresholds)
        {
            _boilerApiClient = boilerApiClient;
            _temperatureConverter = temperatureConverter;
            _logger = logger;
            _temperatureThresholds = temperatureThresholds.Value;
        }

        public async Task Consume(ConsumeContext<TemperatureRequest> context)
        {
            var temperatureFahrenheit = await _boilerApiClient.GetTemperatureAsync();
            var temperatureCelsius = _temperatureConverter.ToCelsius(temperatureFahrenheit);

            ValidateTemperature(temperatureCelsius);

            var response = new TemperatureResponse
            {
                TemperatureCelsius = temperatureCelsius
            };

            await context.RespondAsync(response);
        }

        private void ValidateTemperature(double temperatureCelsius)
        {
            if (temperatureCelsius < _temperatureThresholds.MinTemperature ||
                temperatureCelsius > _temperatureThresholds.MaxTemperature)
            {
                _logger.LogError($"Temperature out of range: {temperatureCelsius}°C");
            }
        }
    }
}
