using MoviesExample.Application.Common.Interfaces;
using MoviesExample.Infrastructure.Persistence;
using MoviesExample.Infrastructure.Persistence.Interceptors;
using MoviesExample.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services
                .AddPooledDbContextFactory<ApplicationDbContext>((s, o) =>
                {
                    o.UseInMemoryDatabase("MoviesExampleDb");
                })
                .AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory<ApplicationDbContext>>();
        }
        else
        {
            services
                .AddPooledDbContextFactory<ApplicationDbContext>((s, o) =>
                {
                    var auditableEntitySaveChangesInterceptor = s.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
                    var configuration = s.GetRequiredService<IConfiguration>();

                    o.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                        .AddInterceptors(auditableEntitySaveChangesInterceptor)
                        .EnableSensitiveDataLogging();
                })
                .AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory<ApplicationDbContext>>();
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddTransient<IDateTime, DateTimeService>();


        return services;
    }
}
