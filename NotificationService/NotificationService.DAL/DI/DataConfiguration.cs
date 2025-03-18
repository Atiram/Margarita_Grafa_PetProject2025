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

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string 'DBConnection' is missing or empty in configuration.");
        }

        if (string.IsNullOrEmpty(connectionStringWithoutDB))
        {
            throw new ArgumentException("Connection string 'DBConnectionWithoutDB' is missing or empty in configuration.");
        }

        if (string.IsNullOrEmpty(scriptPath))
        {
            throw new ArgumentException("Script path 'ScriptPath' is missing or empty in configuration.");
        }

        try
        {
            services.AddScoped<IEventRepository, EventRepository>(provider => new EventRepository(connectionString))
                    .AddScoped<IDbManager, DbManager>(provider => new DbManager(connectionString, connectionStringWithoutDB, scriptPath));
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to register services due to an unexpected error.", ex);
        }
    }
}