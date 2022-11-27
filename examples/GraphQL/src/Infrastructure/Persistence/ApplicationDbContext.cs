using System.Reflection;
using MoviesExample.Application.Common.Interfaces;
using MoviesExample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MoviesExample.Infrastructure.Persistence;

/// <summary>
/// Run the following command to create migrations
///     Add-Migration InitialCreate -OutputDir Persistence/Migrations -StartupProject ../WebApi
///     dotnet ef migrations add InitialCreate --startup-project ../WebApi
///     
/// Run the following command to remove migrations
///     Remove-Migration
///     dotnet ef database update --startup-project ../WebApi
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Movie> Movies => Set<Movie>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
