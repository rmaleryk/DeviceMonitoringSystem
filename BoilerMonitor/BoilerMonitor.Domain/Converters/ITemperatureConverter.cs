namespace BoilerMonitor.Domain.Converters
{
    public interface ITemperatureConverter
    {
        double ToCelsius(double temperatureFahrenheit);
    }
}
