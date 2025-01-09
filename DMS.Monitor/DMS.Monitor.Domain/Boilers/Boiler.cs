using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers.Enums;

namespace DMS.Monitor.Domain.Boilers;

public sealed class Boiler : AggregateRoot
{
    private BoilerStateMachine? _machine;

    public Boiler(Guid id, string name, BoilerState state, BoilerTemperature currentTemperature)
    {
        Id = id;
        Name = name;
        State = state;
        CurrentTemperature = currentTemperature;

        InitStateMachine();
    }

    public string Name { get; private set; }

    public BoilerState State { get; private set; }

    public BoilerTemperature CurrentTemperature { get; private set; }

    public void TurnOn()
    {
        SetState(BoilerStateMachine.Trigger.TurnOn);
        AddDomainEvent(new BoilerTurnedOnEvent(Id));
    }

    public void TurnOff()
    {
        SetState(BoilerStateMachine.Trigger.TurnOff);
        AddDomainEvent(new BoilerTurnedOffEvent(Id));
    }

    public bool UpdateTemperature(double newTemperature, double minTemperature, double maxTemperature)
    {
        CurrentTemperature = BoilerTemperature.FromValue(newTemperature);
        AddDomainEvent(new BoilerTemperatureUpdatedEvent(Id, newTemperature));

        return CurrentTemperature.Validate(minTemperature, maxTemperature);
    }

    private void SetState(BoilerStateMachine.Trigger trigger)
    {
        if (_machine is null)
        {
            InitStateMachine();
        }

        _machine!.ValidateAndFire(trigger);
        State = _machine.State;
    }

    private void InitStateMachine()
    {
        _machine = new BoilerStateMachine(State);
    }

#pragma warning disable CS8618
    private Boiler()
#pragma warning restore CS8618
    {
    }
}
