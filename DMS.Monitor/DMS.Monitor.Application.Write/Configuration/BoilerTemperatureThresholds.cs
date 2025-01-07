using System.ComponentModel.DataAnnotations;

namespace DMS.Monitor.Application.Write.Configuration;

public class BoilerTemperatureThresholds
{
    [Required]
    public double? MinTemperature { get; set; }

    [Required]
    public double? MaxTemperature { get; set; }
}
