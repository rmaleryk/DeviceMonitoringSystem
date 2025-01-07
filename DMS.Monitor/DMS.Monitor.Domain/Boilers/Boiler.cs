using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers.Enums;

namespace DMS.Monitor.Domain.Boilers;

public sealed class Boiler : AggregateRoot
{
    public string Name { get; private set; }

    public BoilerState State { get; private set; }

    public BoilerTemperature CurrentTemperature { get; private set; }

    public Boiler(Guid id, string name)
    {
        Id = id;
        Name = name;
        State = BoilerState.Off;
        CurrentTemperature = BoilerTemperature.Default();
    }

    public void TurnOn()
    {
        if (State == BoilerState.On)
        {
            throw new InvalidOperationException("Boiler is already turned on.");
        }

        State = BoilerState.On;

        AddDomainEvent(new BoilerTurnedOnEvent(Id));
    }

    public void TurnOff()
    {
        if (State == BoilerState.Off)
        {
            throw new InvalidOperationException("Boiler is already turned off.");
        }

        State = BoilerState.Off;

        AddDomainEvent(new BoilerTurnedOffEvent(Id));
    }

    public bool UpdateTemperature(double newTemperature, double minTemperature, double maxTemperature)
    {
        CurrentTemperature = BoilerTemperature.FromValue(newTemperature);
        AddDomainEvent(new BoilerTemperatureUpdatedEvent(Id, newTemperature));

        return CurrentTemperature.Validate(minTemperature, maxTemperature);
    }
}
