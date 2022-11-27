using AutoMapper;
using MediatR;
using MoviesExample.Application.Common.Mappings;
using MoviesExample.Application.Movies.Commands.CreateMovie;
using MoviesExample.Application.Movies.Commands.DeleteMovie;
using MoviesExample.Application.Movies.Commands.UpdateMovie;
using MoviesExample.Domain.Entities;

namespace WebApi.Operations.Movies;

public record CreateMovieInput(string Title, [property: ID(nameof(Genre))] int GenreId) : IMapFrom<CreateMovieCommand>;
//public class CreateMovieInput : IMapFrom<CreateMovieCommand>
//{
//    public string Title { get; set; }

//    [property: ID(nameof(Genre))]
//    public int GenreId { get; set; }
//}

public record UpdateMovieInput([property: ID(nameof(Movie))] int Id, string? Title, [property: ID(nameof(Genre))] int GenreId) : IMapFrom<UpdateMovieCommand>;

public record DeleteMovieInput([property: ID(nameof(Movie))] int Id) : IMapFrom<DeleteMovieCommand>;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class MovieMutations
{
    public async Task<CreateMoviePayload> AddMovieAsync(
        CreateMovieInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<CreateMovieInput, CreateMovieCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<UpdateMoviePayload> UpdateMovieAsync(
        UpdateMovieInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateMovieInput, UpdateMovieCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }
    public async Task<DeleteMoviePayload> DeleteMovieAsync(
        DeleteMovieInput input,
        [Service] IMediator mediator,
        [Service] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<DeleteMovieInput, DeleteMovieCommand>(input);
        return await mediator.Send(request, cancellationToken);
    }
}