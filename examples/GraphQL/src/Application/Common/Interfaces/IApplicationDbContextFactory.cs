namespace MoviesExample.Application.Common.Interfaces;
public interface IApplicationDbContextFactory
{
    IApplicationDbContext CreateDbContext();
}