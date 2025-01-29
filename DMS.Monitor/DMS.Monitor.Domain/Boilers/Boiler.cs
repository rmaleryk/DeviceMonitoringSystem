using DMS.Monitor.Domain.Base;
using DMS.Monitor.Domain.Boilers.Enums;

namespace DMS.Monitor.Domain.Boilers;

public sealed class Boiler : EventSourcedAggregateRoot
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
        RaiseDomainEvent(new BoilerTurnedOnEvent(Id));
    }

    public void TurnOff()
    {
        RaiseDomainEvent(new BoilerTurnedOffEvent(Id));
    }

    public bool UpdateTemperature(double newTemperature, double minTemperature, double maxTemperature)
    {
        RaiseDomainEvent(new BoilerTemperatureUpdatedEvent(Id, newTemperature));
        return CurrentTemperature.Validate(minTemperature, maxTemperature);
    }

    protected override EventSourcedAggregateRoot Apply(DomainEvent e) => Apply((dynamic)e);

    private void SetState(BoilerStateMachine.Trigger trigger)
    {
        if (_machine is null)
        {
            InitStateMachine();
        }

        _machine!.ValidateAndFire(trigger);
        State = _machine.State;
    }

    private Boiler Apply(BoilerTurnedOnEvent _)
    {
        SetState(BoilerStateMachine.Trigger.TurnOn);
        return this;
    }

    private Boiler Apply(BoilerTurnedOffEvent _)
    {
        SetState(BoilerStateMachine.Trigger.TurnOff);
        return this;
    }

    private Boiler Apply(BoilerTemperatureUpdatedEvent e)
    {
        CurrentTemperature = BoilerTemperature.FromValue(e.NewTemperature);
        return this;
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
