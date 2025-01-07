using MediatR;

namespace DMS.Monitor.Application.Write.Boilers.Commands;

public record TurnOnBoilerCommand(Guid Id) : IRequest<CommandResult>;
