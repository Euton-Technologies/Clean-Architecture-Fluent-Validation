using MediatR;
using MoviesExample.Application.Common.Interfaces;
using System.Reflection;
using WebApi.Services;

namespace WebApi;


public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddApplicationServices()
                .AddInfrastructureServices(configuration);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}