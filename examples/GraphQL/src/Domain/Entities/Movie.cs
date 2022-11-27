using System.ComponentModel.DataAnnotations;

namespace MoviesExample.Domain.Entities;

public class Movie : BaseAuditableEntity
{
    [Required]
    [MaxLength(20)]
    public string Title { get; set; } = default!;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = default!;
}
