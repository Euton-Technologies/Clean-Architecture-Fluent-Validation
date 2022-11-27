namespace MoviesExample.Domain.Events;

public class GenreDeletedEvent : BaseEvent
{
    public GenreDeletedEvent(Genre item)
    {
        Item = item;
    }

    public Genre Item { get; }
}
