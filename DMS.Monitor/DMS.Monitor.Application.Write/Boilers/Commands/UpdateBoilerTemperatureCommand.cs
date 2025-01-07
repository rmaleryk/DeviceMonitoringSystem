using MediatR;

namespace DMS.Monitor.Application.Write.Boilers.Commands;

public record UpdateBoilerTemperatureCommand(Guid Id) : IRequest<CommandResult>;
