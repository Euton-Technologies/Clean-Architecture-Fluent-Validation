using MoviesExample.Application.Common.Interfaces;
using MediatR;
using MoviesExample.Application.Common.Models;
using MoviesExample.Application.Genres.Models;
using CleanArchitecture.ValidationRules.Types;

namespace MoviesExample.Application.Genres.Commands.UpdateGenre;

[ExtendValidation(typeof(Genre))]
public record UpdateGenreCommand : IRequest<UpdateGenrePayload>
{
    public int Id { get; init; }

    public string? Name { get; init; }
}
public class UpdateGenrePayload : GenrePayloadBase
{
    public UpdateGenrePayload(Genre genre) : base(genre)
    { }

    public UpdateGenrePayload(UserError error)
        : base(new[] { error })
    { }
}

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, UpdateGenrePayload>
{
    private readonly IApplicationDbContext _context;

    public UpdateGenreCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateGenrePayload> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Genres
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return new UpdateGenrePayload(new UserError("Genre with id not found.", "GENRE_NOT_FOUND"));
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);


        return new UpdateGenrePayload(entity);
    }
}
