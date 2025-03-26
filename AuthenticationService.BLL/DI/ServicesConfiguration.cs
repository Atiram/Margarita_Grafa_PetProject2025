using AuthenticationService.BLL.Services;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.DI;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>()
                .RegisterDataRepositories();
    }
}
