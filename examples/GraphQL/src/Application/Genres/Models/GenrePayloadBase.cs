using MoviesExample.Application.Common.Models;

namespace MoviesExample.Application.Genres.Models;
public class GenrePayloadBase : Payload
{
    public GenrePayloadBase(Genre genre)
    {
        Genre = genre;
    }

    public GenrePayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {
    }

    public Genre? Genre { get; }
}