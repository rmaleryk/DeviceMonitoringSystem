using DMS.Monitor.Domain.Boilers.Enums;
using Stateless;

namespace DMS.Monitor.Domain.Boilers;

internal sealed class BoilerStateMachine
{
    private readonly StateMachine<BoilerState, Trigger> _machine;

    public BoilerStateMachine(BoilerState initState)
    {
        _machine = InitStateMachine(initState);
    }

    public BoilerState State => _machine.State;

    public void ValidateAndFire(Trigger trigger)
    {
        if (!_machine.CanFire(trigger))
        {
            throw new InvalidOperationException(
                $"Boiler is in incorrect status to trigger '{trigger}'. Current state: {_machine.State}");
        }

        _machine.Fire(trigger);
    }

    private static StateMachine<BoilerState, Trigger> InitStateMachine(BoilerState state)
    {
        var machine = new StateMachine<BoilerState, Trigger>(state);

        machine.Configure(BoilerState.Off)
            .Permit(Trigger.TurnOn, BoilerState.On);

        machine.Configure(BoilerState.On)
            .Permit(Trigger.TurnOff, BoilerState.Off);

        return machine;
    }

    internal enum Trigger
    {
        TurnOn,
        TurnOff
    }
}
