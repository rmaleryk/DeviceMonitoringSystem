using Microsoft.AspNetCore.Mvc;

namespace DMS.FakeDevices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoilerController(ILogger<BoilerController> logger) : ControllerBase
{
    private readonly Random _random = new();

    [HttpGet("temperature")]
    public IActionResult Get()
    {
        var temperatureFahrenheit = _random.Next(14, 248);

        logger.LogInformation("Generated temperature {Temperature}°F", temperatureFahrenheit);

        return Ok(temperatureFahrenheit);
    }
}
