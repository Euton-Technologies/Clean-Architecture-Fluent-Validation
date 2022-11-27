namespace MoviesExample.Domain.Events;

public class GenreCreatedEvent : BaseEvent
{
    public GenreCreatedEvent(Genre item)
    {
        Item = item;
    }

    public Genre Item { get; }
}
