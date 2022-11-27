using MediatR;
using MoviesExample.Application.Movies.Queries;
using MoviesExample.Domain.Entities;

namespace WebApi.Operations.Movies;

[ExtendObjectType(OperationTypeNames.Query)]
public class GetMoviesQueries
{
    /// <summary>
    /// Gets all of the movies
    /// </summary>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Movie>> GetMovies(
       [Service] IMediator mediator)
    {
        return await mediator.Send(new GetMovieQuery());
    }
}
