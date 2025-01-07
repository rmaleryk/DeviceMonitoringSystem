using System.ComponentModel.DataAnnotations;

namespace DMS.Client.Api.Configuration;

public class BoilerConfiguration
{
    [Required]
    public Guid? Id { get; set; }
}
