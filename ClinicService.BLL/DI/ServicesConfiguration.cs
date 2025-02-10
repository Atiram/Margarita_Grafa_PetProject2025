using ClinicService.BLL.Services;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicService.BLL.DI;
public static class ServicesConfiguration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();
        //.AddScoped<IPatientService, PatientService>()
        //.AddScoped<IAppointmentService, AppointmentService>();
    }
}
