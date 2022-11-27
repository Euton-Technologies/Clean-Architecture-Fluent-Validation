using MoviesExample.Domain.Entities;
using WebApi.DataLoaders;

namespace WebApi.Operations.Genres;

[Node]
[ExtendObjectType(typeof(Genre))]
public class GenreNode
{
    [NodeResolver]
    public static Task<Genre> GetGenreByIdAsync(
        int id,
        GenreByIdDataLoader GenreById,
        CancellationToken cancellationToken)
        => GenreById.LoadAsync(id, cancellationToken);
}