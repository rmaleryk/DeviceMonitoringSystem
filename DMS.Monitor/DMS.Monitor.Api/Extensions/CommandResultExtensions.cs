using DMS.Monitor.Api.Common;
using DMS.Monitor.Application.Write;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Monitor.Api.Extensions;

public static class CommandResultExtensions
{
    public static IActionResult ToHttpResult(this CommandResult commandResult)
    {
        IActionResult result = commandResult.Status switch
        {
            CommandStatus.Failed => new BadRequestObjectResult(CreateFailedCommandResponse()),
            CommandStatus.Completed => new OkObjectResult(CreateSuccessfulCommandResponse()),
            CommandStatus.Accepted => new StatusCodeResult(StatusCodes.Status202Accepted),
            CommandStatus.Created
                => new ObjectResult(CreateSuccessfulCommandResponse()) { StatusCode = StatusCodes.Status201Created },
            CommandStatus.InvalidState => new ConflictObjectResult(CreateFailedCommandResponse()),
            _ => new OkResult()
        };

        return result;

        CommandResponse CreateFailedCommandResponse()
            => new(commandResult.Errors, commandResult.Status, commandResult.AggregateId);

        CommandResponse CreateSuccessfulCommandResponse()
            => new(commandResult.Status, commandResult.AggregateId);
    }
}