namespace DMS.Monitor.Application.Write;

public record CommandResult
{
    private CommandResult(CommandStatus status)
        : this(status, aggregateId: null)
    {
    }

    private CommandResult(CommandStatus status, string error)
        : this(status, null, error)
    {
    }

    private CommandResult(CommandStatus status, Guid? aggregateId)
    {
        Status = status;
        AggregateId = aggregateId;
        Errors = new List<string>();
    }

    private CommandResult(CommandStatus status, Guid? aggregateId, string error)
        : this(status, aggregateId, new List<string> { error })
    {
    }

    private CommandResult(CommandStatus status, Guid? aggregateId, IList<string> errors)
        : this(status, aggregateId)
    {
        if (errors.Count > 0)
        {
            if (Status is not (CommandStatus.Failed or CommandStatus.Accepted or CommandStatus.InvalidState))
            {
                throw new InvalidOperationException(
                    $"You can add errors only when status is {CommandStatus.Failed} or {CommandStatus.Accepted}.");
            }

            Errors = errors;
        }
    }

    public CommandStatus Status { get; }

    public Guid? AggregateId { get; }

    public IList<string> Errors { get; }

    public static CommandResult Completed()
        => new(CommandStatus.Completed);

    public static CommandResult Completed(Guid aggregateId)
        => new(CommandStatus.Completed, aggregateId);

    public static CommandResult Created(Guid aggregateId)
        => new(CommandStatus.Created, aggregateId);

    public static CommandResult Accepted(Guid aggregateId, string error)
        => new(CommandStatus.Accepted, aggregateId, error);

    public static CommandResult Accepted(Guid aggregateId, List<string> errors)
        => new(CommandStatus.Accepted, aggregateId, errors);

    public static CommandResult Failed(Guid aggregateId, string error)
        => new(CommandStatus.Failed, aggregateId, error);

    public static CommandResult Failed(Guid aggregateId, List<string> errors)
        => new(CommandStatus.Failed, aggregateId, errors);

    public static CommandResult Conflict(Guid aggregateId, string error)
        => new(CommandStatus.InvalidState, aggregateId, error);

    public static CommandResult Conflict(string error)
        => new(CommandStatus.InvalidState, null, error);
}
