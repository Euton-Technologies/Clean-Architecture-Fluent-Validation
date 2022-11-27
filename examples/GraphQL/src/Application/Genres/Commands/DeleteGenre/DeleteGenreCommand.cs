using MoviesExample.Application.Common.Interfaces;
using MediatR;
using MoviesExample.Application.Common.Models;

namespace MoviesExample.Application.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(int Id) : IRequest<DeleteGenrePayload>;


public class DeleteGenrePayload : DeletePayload
{
    public DeleteGenrePayload(bool success)
    {
        Success = success;
    }

    public DeleteGenrePayload(UserError error)
        : base(new[] { error })
    { }
    public bool Success { get; }
}
public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, DeleteGenrePayload>
{
    private readonly IApplicationDbContext _context;

    public DeleteGenreCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteGenrePayload> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Genres
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return new DeleteGenrePayload(new UserError("Genre with id not found.", "GENRE_NOT_FOUND"));
        }

        _context.Genres.Remove(entity);

        entity.AddDomainEvent(new GenreDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteGenrePayload(true);
    }
}
