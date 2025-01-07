using System.ComponentModel.DataAnnotations;

namespace DMS.Monitor.Domain.Base;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }
}
