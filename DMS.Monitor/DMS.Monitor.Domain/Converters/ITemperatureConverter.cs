namespace DMS.Monitor.Domain.Converters;

public interface ITemperatureConverter
{
    double ToCelsius(double temperatureFahrenheit);
}
