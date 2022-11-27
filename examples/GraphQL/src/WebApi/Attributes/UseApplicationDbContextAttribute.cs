using MoviesExample.Application.Common.Interfaces;

namespace WebApi.Attributes;

public class UseApplicationDbContextAttribute : UseDbContextAttribute
{
    public UseApplicationDbContextAttribute() : base(typeof(IApplicationDbContext))
    {
    }
}