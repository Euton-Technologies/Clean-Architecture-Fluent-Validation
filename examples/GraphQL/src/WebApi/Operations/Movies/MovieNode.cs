using MoviesExample.Domain.Entities;
using WebApi.DataLoaders;

namespace WebApi.Operations.Movies;

[Node]
[ExtendObjectType(typeof(Movie))]
public class MovieNode
{
    [NodeResolver]
    public static Task<Movie> GetMovieByIdAsync(
        int id,
        MovieByIdDataLoader MovieById,
        CancellationToken cancellationToken)
        => MovieById.LoadAsync(id, cancellationToken);
}