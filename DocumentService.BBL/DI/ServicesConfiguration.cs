using System.Reflection;
using DocumentService.BBL.Services;
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
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)))
                .AddScoped<IAzureBlobService, AzureBlobService>(provider => new AzureBlobService(configuration))
                .AddScoped<IFileService, FileService>()
                .RegisterDataRepositories(configuration);
    }
}

