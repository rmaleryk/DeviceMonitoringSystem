namespace DMS.Monitor.Application.Write;

public enum CommandStatus : byte
{
    Completed = 1,
    Created,
    Accepted,
    Failed,
    InvalidState
}
