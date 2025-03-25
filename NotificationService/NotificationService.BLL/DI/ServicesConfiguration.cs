using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.BLL.Services;
using NotificationService.BLL.Services.Interfaces;
using NotificationService.DAL.DI;

namespace NotificationService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventService, EventService>()
            .AddScoped<IEmailService, EmailService>()
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ServicesConfiguration).Assembly))
            .RegisterRepositories(configuration);
    }
}