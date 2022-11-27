using MediatR;
using Microsoft.Extensions.Logging;

namespace MoviesExample.Application.Genres.EventHandlers;

public class GenreCreatedEventHandler : INotificationHandler<GenreCreatedEvent>
{
    private readonly ILogger<GenreCreatedEventHandler> _logger;

    public GenreCreatedEventHandler(ILogger<GenreCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(GenreCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("MoviesExample Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
