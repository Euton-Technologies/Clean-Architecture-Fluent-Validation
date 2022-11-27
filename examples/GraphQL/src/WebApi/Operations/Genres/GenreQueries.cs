using MediatR;
using MoviesExample.Application.Genres.Queries;
using MoviesExample.Domain.Entities;

namespace WebApi.Operations.Genres;

[ExtendObjectType(OperationTypeNames.Query)]
public class GetGenresQueries
{
    /// <summary>
    /// Gets access to all the people known to this service.
    /// </summary>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Genre>> GetGenres(
       [Service] IMediator mediator)
    {
        return await mediator.Send(new GetGenreQuery());
    }
}
