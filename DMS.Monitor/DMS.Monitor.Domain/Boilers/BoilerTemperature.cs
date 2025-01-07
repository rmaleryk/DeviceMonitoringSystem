namespace DMS.Monitor.Domain.Boilers;

public class BoilerTemperature
{
    public double Value { get; private set; }

    private BoilerTemperature(double value)
    {
        Value = value;
    }

    public bool Validate(double minTemperature, double maxTemperature) =>
        Value >= minTemperature && Value <= maxTemperature;

    public static BoilerTemperature Default() => new(0);

    public static BoilerTemperature FromValue(double value) => new(value);

    public override string ToString() => $"{Value} °C";
}
