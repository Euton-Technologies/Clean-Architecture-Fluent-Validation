using Microsoft.EntityFrameworkCore;
using MoviesExample.Application.Common.Interfaces;

namespace MoviesExample.Infrastructure.Persistence;
public class ApplicationDbContextFactory<TContext> : IApplicationDbContextFactory where TContext : ApplicationDbContext
{
    private readonly IDbContextFactory<TContext> _factory;

    public ApplicationDbContextFactory(IDbContextFactory<TContext> factory)
    {
        _factory = factory;
    }

    public IApplicationDbContext CreateDbContext() => _factory.CreateDbContext();
}