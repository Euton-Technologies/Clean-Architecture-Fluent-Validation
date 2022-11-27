using MoviesExample.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoviesExample.Application.Common.Models;

namespace MoviesExample.Application.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand(int Id) : IRequest<DeleteMoviePayload>;


public class DeleteMoviePayload : DeletePayload
{
    public DeleteMoviePayload(bool success)
    {
        Success = success;
    }

    public DeleteMoviePayload(UserError error)
        : base(new[] { error })
    { }
    public bool Success { get; }
}
public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, DeleteMoviePayload>
{
    private readonly IApplicationDbContext _context;

    public DeleteMovieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteMoviePayload> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Movies
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return new DeleteMoviePayload(new UserError("Movie with id not found.", "MOVIE_NOT_FOUND"));
        }

        _context.Movies.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteMoviePayload(true);
    }
}
