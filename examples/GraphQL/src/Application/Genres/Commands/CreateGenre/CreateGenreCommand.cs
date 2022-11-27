using MoviesExample.Application.Common.Interfaces;
using MediatR;
using MoviesExample.Application.Genres.Models;
using MoviesExample.Application.Common.Models;
using EutonTechnologies.ValidationRules.Types;

namespace MoviesExample.Application.Genres.Commands.CreateGenre;

[ExtendValidation(typeof(Genre))]
public record CreateGenreCommand : IRequest<CreateGenrePayload>
{

    public string? Name { get; init; }
}

public class CreateGenrePayload : GenrePayloadBase
{
    public CreateGenrePayload(Genre genre) : base(genre)
    { }

    public CreateGenrePayload(UserError error)
        : base(new[] { error })
    { }
}

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, CreateGenrePayload>
{
    private readonly IApplicationDbContext _context;

    public CreateGenreCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateGenrePayload> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = new Genre
        {
            Name = request.Name
        };

        entity.AddDomainEvent(new GenreCreatedEvent(entity));

        _context.Genres.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateGenrePayload(entity);
    }
}
