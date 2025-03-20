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
        services.AddScoped<IEventRepository, EventRepository>()
                .AddScoped<IDbManager, DbManager>();
    }
}