using DMS.Monitor.Application.Write;

namespace DMS.Monitor.Api.Common;

public class CommandResponse
{
    public CommandResponse(IList<string> errors, CommandStatus status, Guid? objectId)
    {
        Errors = errors;
        Status = status.ToString();
        ObjectId = objectId?.ToString();
    }

    public CommandResponse(CommandStatus status, Guid? objectId)
        : this(new List<string>(), status, objectId)
    {
    }

    public IList<string> Errors { get; init; }

    public string Status { get; init; }

    public string? ObjectId { get; init; }
}
