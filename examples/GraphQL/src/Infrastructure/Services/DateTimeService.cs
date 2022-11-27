using MoviesExample.Application.Common.Interfaces;

namespace MoviesExample.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
