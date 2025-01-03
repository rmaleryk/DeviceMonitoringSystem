using System.ComponentModel.DataAnnotations;

namespace DMS.Monitor.Application.Configuration;

public class TemperatureThresholds
{
    [Required]
    public double? MinTemperature { get; set; }

    [Required]
    public double? MaxTemperature { get; set; }
}
