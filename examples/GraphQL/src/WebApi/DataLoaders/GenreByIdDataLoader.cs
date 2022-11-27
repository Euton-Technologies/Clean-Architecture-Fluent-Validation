using Microsoft.EntityFrameworkCore;
using MoviesExample.Application.Common.Interfaces;
using MoviesExample.Domain.Entities;

namespace WebApi.DataLoaders;

public class GenreByIdDataLoader : BatchDataLoader<int, Genre>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GenreByIdDataLoader(
        IApplicationDbContextFactory dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory ??
            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Genre>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using IApplicationDbContext dbContext =
            _dbContextFactory.CreateDbContext();

        return await dbContext.Genres
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}