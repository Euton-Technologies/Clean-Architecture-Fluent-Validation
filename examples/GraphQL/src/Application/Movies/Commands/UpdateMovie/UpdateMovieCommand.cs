using MoviesExample.Application.Common.Interfaces;
using MediatR;
using MoviesExample.Application.Common.Models;
using MoviesExample.Application.Movies.Models;
using EutonTechnologies.ValidationRules.Types;

namespace MoviesExample.Application.Movies.Commands.UpdateMovie;

[ExtendValidation(typeof(Movie))]
public record UpdateMovieCommand : IRequest<UpdateMoviePayload>
{
    public int Id { get; init; }

    public int GenreId { get; init; }

    public string? Title { get; init; }
}

public class UpdateMoviePayload : MoviePayloadBase
{
    public UpdateMoviePayload(Movie Movie) : base(Movie)
    { }

    public UpdateMoviePayload(UserError error)
        : base(new[] { error })
    { }
}
public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, UpdateMoviePayload>
{
    private readonly IApplicationDbContext _context;

    public UpdateMovieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateMoviePayload> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Movies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return new UpdateMoviePayload(new UserError("Movie with id not found.", "MOVIE_NOT_FOUND"));
        }

        entity.Title = request.Title;
        var genre = _context.Genres.FirstOrDefault(g => g.Id == request.GenreId);
        if (genre == null)
        {
            return new UpdateMoviePayload(new UserError("Genre with id not found.", "GENRE_NOT_FOUND"));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateMoviePayload(entity);
    }
}
