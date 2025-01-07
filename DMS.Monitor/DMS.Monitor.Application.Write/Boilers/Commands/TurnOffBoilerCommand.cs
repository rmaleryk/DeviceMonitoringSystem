using MediatR;

namespace DMS.Monitor.Application.Write.Boilers.Commands;

public record TurnOffBoilerCommand(Guid Id) : IRequest<CommandResult>;
