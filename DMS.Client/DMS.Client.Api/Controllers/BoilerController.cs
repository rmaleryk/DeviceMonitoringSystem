using DMS.Client.Api.Caching;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Client.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoilerController(BoilerTemperatureCache boilerTemperatureCache) : ControllerBase
{
    [HttpGet("temperature")]
    public IActionResult GetTemperature()
    {
        var boilerTemperature = boilerTemperatureCache.GetTemperature();
        return Ok(new
        {
            Value = boilerTemperature
        });
    }
}
