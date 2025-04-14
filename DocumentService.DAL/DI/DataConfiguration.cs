using DocumentService.DAL.Repositories;
using DocumentService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DocumentService.DAL.DI;
public static class DataConfiguration
{
    private const string ConnectionStringVariableName = "MONGO_CONNECTION_STRING";
    private const string MongoConnectionString = "MongoSettings:ConnectionString";

    public static void RegisterDataRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(MongoConnectionString)?.Value;
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariableName);
        }
        var mongoClient = new MongoClient(connectionString);

        services.AddSingleton<IMongoClient>(mongoClient)
                .AddScoped<IFileRepository, FileRepository>();
    }
}