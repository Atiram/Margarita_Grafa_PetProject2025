using System.Reflection;
using ClinicService.BLL.RabbitMqProducer;
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
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AppMappingProfile)));
        services.AddScoped<IDoctorService, DoctorService>()
            .AddScoped<IPatientService, PatientService>()
            .AddScoped<IAppointmentService, AppointmentService>()
            .AddScoped<IAppointmentResultService, AppointmentResultService>()
            .AddScoped<INotificationHttpClient, NotificationHttpClient>()
            .AddScoped<IRabbitMqService, RabbitMqService>()
            .AddScoped<IGeneratePdfService, GeneratePdfService>()
            .AddScoped<IDocumentService, DocumentService>()
            .AddScoped<IAppointmentReminderService, AppointmentReminderService>()
            .AddScoped<IBackgroundWorkerService, BackgroundWorkerService>()
            .AddHttpClient()
            .RegisterDataRepositories(configuration);
    }
}
