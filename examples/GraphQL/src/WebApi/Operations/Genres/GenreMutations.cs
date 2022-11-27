using AutoMapper;
using MediatR;
using MoviesExample.Application.Common.Mappings;
using MoviesExample.Application.Genres.Commands.CreateGenre;
using MoviesExample.Application.Genres.Commands.DeleteGenre;
using MoviesExample.Application.Genres.Commands.UpdateGenre;
using MoviesExample.Domain.Entities;

namespace WebApi.Operations.Genres;

public record CreateGenreInput(string Name) : IMapFrom<CreateGenreCommand>;

public record UpdateGenreInput([property: ID(nameof(Genre))] int Id, string? Name) : IMapFrom<UpdateGenreCommand>;

public record DeleteGenreInput([property: ID(nameof(Genre))] int Id) : IMapFrom<DeleteGenreCommand>;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class GenreMutations
{
    public async Task<CreateGenrePayload> AddGenreAsync(
        CreateGenreInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<CreateGenreInput, CreateGenreCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<UpdateGenrePayload> UpdateGenreAsync(
        UpdateGenreInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateGenreInput, UpdateGenreCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }
    public async Task<DeleteGenrePayload> DeleteGenreAsync(
        DeleteGenreInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<DeleteGenreInput, DeleteGenreCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }
}