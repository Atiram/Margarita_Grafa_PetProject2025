using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.DAL.DBManager;
using NotificationService.DAL.Repositories;
using NotificationService.DAL.Repositories.Interfaces;

namespace NotificationService.DAL.DI;
public static class DataConfiguration
{
    public static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DBConnection");
        string? connectionStringWithoutDB = configuration.GetConnectionString("DBConnectionWithoutDB");
        string? scriptPath = configuration.GetSection("ScriptPath").Value;
        services.AddScoped<IEventRepository, EventRepository>(provider => new EventRepository(connectionString))
            .AddScoped<IDbManager, DbManager>(provider => new DbManager(connectionString, connectionStringWithoutDB, scriptPath));
    }
}