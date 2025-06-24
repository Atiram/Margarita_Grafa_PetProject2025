using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeService.BLL.Services.Interfaces;
using OfficeService.BLL.Utilities.Mapping;
using OfficeService.DAL.DI;

namespace OfficeService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOfficeService, OfficeService.BLL.Services.OfficeService>()
            .AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)))
            .RegisterDataRepositories(configuration);
    }
}
