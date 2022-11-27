using MediatR;
using Microsoft.EntityFrameworkCore;
using MoviesExample.Application.Common.Interfaces;

namespace MoviesExample.Application.Movies.Queries;

public record GetMovieQuery : IRequest<IQueryable<Movie>>;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, IQueryable<Movie>>
{
    private readonly IApplicationDbContext _context;

    public GetMovieQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Movie>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        return _context.Movies.AsNoTracking();
    }
}