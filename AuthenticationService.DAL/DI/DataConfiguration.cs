using AuthenticationService.DAL.Repositories;
using AuthenticationService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.DAL.DI;
public static class DataConfiguration
{
    public static void RegisterDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}