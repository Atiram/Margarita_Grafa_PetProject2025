using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeService.DAL.MongoDb;
using OfficeService.DAL.Repositories;
using OfficeService.DAL.Repositories.Interfaces;

namespace OfficeService.DAL.DI;
public static class DataConfiguration
{
    public static void RegisterDataRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<IOfficeRepository, OfficeRepository>();
    }
}
