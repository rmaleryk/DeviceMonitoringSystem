using DMS.Client.Api.Caching;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Client.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoilersController(BoilerTemperatureCache boilerTemperatureCache) : ControllerBase
{
    [HttpGet("{id:guid}/temperature")]
    public IActionResult GetTemperature([FromRoute] Guid id)
    {
        var boilerTemperature = boilerTemperatureCache.GetTemperature(id);
        return Ok(new
        {
            Value = boilerTemperature
        });
    }
}
