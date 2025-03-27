using AuthenticationService.DAL.Repositories;
using AuthenticationService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AuthenticationService.DAL.DI;
public static class DataConfiguration
{
    public static void RegisterDataRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("MongoSettings:ConnectionString").Value;
        var mongoClient = new MongoClient(connectionString);

        services.AddSingleton<IMongoClient>(mongoClient);
        //var f = configuration.GetSection("MongoDbSettings");
        //services.Configure<MongoDbSettings>(configuration.GetSection("MongoSettings"));
        services.AddScoped<IUserRepository, UserRepository>();
    }
}