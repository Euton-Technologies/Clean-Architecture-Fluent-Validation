using MoviesExample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MoviesExample.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                if(!await _context.Database.EnsureCreatedAsync())
                {
                    await _context.Database.MigrateAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Movies.Any())
        {
            var actionGenre = new Genre { Name = "Action" };
            var comedyGenre = new Genre { Name = "Comedy" };
            var dramaGenre = new Genre { Name = "Drama" };
            _context.Genres.Add(actionGenre);
            _context.Genres.Add(comedyGenre);
            _context.Genres.Add(dramaGenre);
            _context.Movies.Add(new Movie
            {
                Title = "Super Cool Movie",
                Genre = actionGenre,
            });
            _context.Movies.Add(new Movie
            {
                Title = "Super Cool Drama",
                Genre = dramaGenre,
            });
            _context.Movies.Add(new Movie
            {
                Title = "Really Cool Comedy",
                Genre = comedyGenre,
            });

            await _context.SaveChangesAsync();
        }
    }
}
