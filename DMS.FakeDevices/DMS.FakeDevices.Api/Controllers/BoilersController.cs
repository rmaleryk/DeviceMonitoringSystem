using Microsoft.AspNetCore.Mvc;

namespace DMS.FakeDevices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoilersController(ILogger<BoilersController> logger) : ControllerBase
{
    private readonly Random _random = new();

    [HttpGet("{id:guid}/temperature")]
    public IActionResult Get([FromRoute] Guid id)
    {
        var temperatureFahrenheit = _random.Next(14, 248);

        logger.LogInformation(
            "Generated temperature {Temperature}°F for the boiler with id {Id}",
            temperatureFahrenheit,
            id);

        return Ok(temperatureFahrenheit);
    }
}
