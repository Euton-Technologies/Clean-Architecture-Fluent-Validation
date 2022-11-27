using MoviesExample.Application.Common.Models;

namespace MoviesExample.Application.Movies.Models;
public class MoviePayloadBase : Payload
{
    public MoviePayloadBase(Movie movie)
    {
        Movie = movie;
    }

    public MoviePayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {
    }

    public Movie? Movie { get; }
}