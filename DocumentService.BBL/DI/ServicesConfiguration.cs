using System.Reflection;
using DocumentService.BBL.Services.Interfaces;
using DocumentService.BBL.Utilities.Mapping;
using DocumentService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.BBL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));
        services.AddScoped<IDocumentService, DocumentService>()
            .RegisterDataRepositories(configuration);
    }
}

