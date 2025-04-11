using DocumentService.DAL.Repositories;
using DocumentService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DocumentService.DAL.DI;
public static class DataConfiguration
{
    public static void RegisterDataRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("MongoSettings:ConnectionString")?.Value;
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        }
        var mongoClient = new MongoClient(connectionString);

        services.AddSingleton<IMongoClient>(mongoClient)
                .AddScoped<IFileRepository, FileRepository>();
    }
}