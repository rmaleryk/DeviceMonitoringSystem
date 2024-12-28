using Microsoft.AspNetCore.Mvc;

namespace DMS.FakeDevices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoilerController : ControllerBase
{
    private readonly Random _random = new();
    private readonly ILogger<BoilerController> _logger;

    public BoilerController(ILogger<BoilerController> logger)
    {
        _logger = logger;
    }

    [HttpGet("temperature")]
    public IActionResult Get()
    {
        var temperatureFahrenheit = _random.Next(14, 248);

        _logger.LogInformation("Generated temperature {Temperature}°F", temperatureFahrenheit);

        return Ok(temperatureFahrenheit);
    }
}
