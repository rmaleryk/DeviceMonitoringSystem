using System.ComponentModel.DataAnnotations;

namespace DMS.Monitor.Application.Configuration;

public class RabbitMqSettings
{
    [Required]
    public string? Host { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}