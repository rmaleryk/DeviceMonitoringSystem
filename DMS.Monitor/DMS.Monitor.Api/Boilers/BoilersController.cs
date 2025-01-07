using DMS.Monitor.Api.Extensions;
using DMS.Monitor.Application.Read.Boilers.Queries;
using DMS.Monitor.Application.Write.Boilers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Monitor.Api.Boilers;

[Route("api/[controller]")]
[ApiController]
public class BoilersController : ControllerBase
{
    private readonly ISender _sender;

    public BoilersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetBoilersQuery();
        var response = await _sender.Send(query, cancellationToken);
        return response.ToActionResult();
    }

    [HttpGet("{id:guid:required}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetBoilerQuery(id);
        var response = await _sender.Send(query, cancellationToken);
        return response.ToActionResult();
    }

    [HttpPost("{id:guid:required}/turn-on")]
    public async Task<IActionResult> TurnOn(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new TurnOnBoilerCommand(id);
        var response = await _sender.Send(command, cancellationToken);
        return response.ToHttpResult();
    }

    [HttpPost("{id:guid:required}/turn-off")]
    public async Task<IActionResult> TurnOff(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new TurnOffBoilerCommand(id);
        var response = await _sender.Send(command, cancellationToken);
        return response.ToHttpResult();
    }
}
