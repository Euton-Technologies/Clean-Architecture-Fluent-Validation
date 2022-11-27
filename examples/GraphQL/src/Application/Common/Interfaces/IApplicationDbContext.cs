using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MoviesExample.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable, IAsyncDisposable
{
    DbSet<Movie> Movies { get; }

    DbSet<Genre> Genres { get; }

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
