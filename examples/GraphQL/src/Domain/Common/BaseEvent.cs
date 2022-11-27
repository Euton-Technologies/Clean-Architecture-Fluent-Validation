using MediatR;

namespace MoviesExample.Domain.Common;

public abstract class BaseEvent : INotification
{
    public string Name { get; } = default!;
}
