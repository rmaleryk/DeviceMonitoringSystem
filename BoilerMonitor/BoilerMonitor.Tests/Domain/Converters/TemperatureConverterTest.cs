using BoilerMonitor.Domain.Converters;
using FluentAssertions;

namespace BoilerMonitor.Tests.Domain.Converters
{
    public class TemperatureConverterTest
    {
        private readonly TemperatureConverter _temperatureConverter;

        public TemperatureConverterTest()
        {
            _temperatureConverter = new TemperatureConverter();
        }

        [Theory]
        [InlineData(14, -10)]
        [InlineData(50, 10)]
        [InlineData(212, 100)]
        public void ToCelsius_ConvertFahrenheitToCelsius(
            double temperatureFahrenheit,
            double temperatureCelsius)
        {
            var actual = _temperatureConverter.ToCelsius(temperatureFahrenheit);

            actual.Should().Be(temperatureCelsius);            
        }
    }
}
