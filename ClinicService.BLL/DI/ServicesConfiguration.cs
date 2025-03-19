using System.Reflection;
using ClinicService.BLL.Services;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Mapping;
using ClinicService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? eventUrl = configuration.GetSection("EventUrl").Value;

        if (string.IsNullOrEmpty(eventUrl))
        {
            throw new ArgumentException("Section 'EventUrl' is missing or empty in configuration.");
        }
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));
        services.AddScoped<IDoctorService, DoctorService>()
            .AddScoped<IPatientService, PatientService>()
            .AddScoped<IAppointmentService, AppointmentService>()
            .AddScoped<INotificationHttpClient, NotificationHttpClient>(provider => new NotificationHttpClient(eventUrl))
            .RegisterDataRepositories(configuration);
    }
}
