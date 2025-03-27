using AuthenticationService.BLL.Services;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>()
                .RegisterDataRepositories(configuration);
    }
}
