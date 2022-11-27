using MediatR;
using Microsoft.EntityFrameworkCore;
using MoviesExample.Application.Common.Interfaces;

namespace MoviesExample.Application.Genres.Queries;

public record GetGenreQuery : IRequest<IQueryable<Genre>>;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, IQueryable<Genre>>
{
    private readonly IApplicationDbContext _context;

    public GetGenreQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Genre>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
    {
        return _context.Genres.AsNoTracking();
    }
}