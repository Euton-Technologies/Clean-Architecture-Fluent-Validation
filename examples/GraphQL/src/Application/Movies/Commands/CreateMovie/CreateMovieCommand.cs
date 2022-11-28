using MoviesExample.Application.Common.Interfaces;
using MediatR;
using MoviesExample.Application.Common.Models;
using MoviesExample.Application.Movies.Models;
using CleanArchitecture.ValidationRules.Types;

namespace MoviesExample.Application.Movies.Commands.CreateMovie;

[ExtendValidation(typeof(Movie))]
public record CreateMovieCommand : IRequest<CreateMoviePayload>
{
    public string? Title { get; init; }
    public int GenreId { get; init; }
}

public class CreateMoviePayload : MoviePayloadBase
{
    public CreateMoviePayload(Movie Movie) : base(Movie)
    { }

    public CreateMoviePayload(UserError error)
        : base(new[] { error })
    { }
}


public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, CreateMoviePayload>
{
    private readonly IApplicationDbContext _context;

    public CreateMovieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateMoviePayload> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = new Movie();

        entity.Title = request.Title;
        var genre = _context.Genres.FirstOrDefault(g => g.Id == request.GenreId);
        if(genre == null)
        {
            return new CreateMoviePayload(new UserError("Genre with id not found.", "GENRE_NOT_FOUND"));
        }
        entity.Genre = genre;

        _context.Movies.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateMoviePayload(entity);
    }
}
