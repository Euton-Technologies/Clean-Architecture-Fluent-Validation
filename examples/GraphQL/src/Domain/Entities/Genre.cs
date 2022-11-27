using System.ComponentModel.DataAnnotations;

namespace MoviesExample.Domain.Entities;

public class Genre : BaseAuditableEntity
{
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }
}
