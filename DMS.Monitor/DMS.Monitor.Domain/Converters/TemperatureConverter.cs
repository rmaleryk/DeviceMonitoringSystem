namespace DMS.Monitor.Domain.Converters;

internal class TemperatureConverter : ITemperatureConverter
{
    public double ToCelsius(double temperatureFahrenheit)
    {
        return (temperatureFahrenheit - 32) * 5.0 / 9.0;
    }
}
