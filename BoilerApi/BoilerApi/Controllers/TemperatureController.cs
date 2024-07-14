using Microsoft.AspNetCore.Mvc;

namespace BoilerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly Random _random = new();
        private readonly ILogger<TemperatureController> _logger;

        public TemperatureController(ILogger<TemperatureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var temperatureFahrenheit = _random.Next(14, 248);

            _logger.LogInformation($"Generated temperature {temperatureFahrenheit}°F");

            return Ok(temperatureFahrenheit);
        }
    }
}
