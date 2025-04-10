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
    private const string AzureConnectionString = "AzureBlobStorage";
    public static void RegisterBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        var azureConnectionString = configuration.GetConnectionString(AzureConnectionString); 
        var azureContName = configuration.GetSection("BlobStorageContainerName").Value;
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));
        services.AddTransient<AzureBlobService>(x => new AzureBlobService(azureConnectionString, azureContName));
        services.AddScoped<IFileService, FileService>()
                .RegisterDataRepositories(configuration);
    }
}

