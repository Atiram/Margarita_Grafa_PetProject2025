using ClinicService.DAL.Data;
using ClinicService.DAL.Repositories;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicService.DAL.DI;

public static class DataConfiguration
{
    public static void RegisterDataRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClinicDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

        services.AddScoped<IDoctorRepository, DoctorRepository>()
            .AddScoped<IPatientRepository, PatientRepository>()
            .AddScoped<IAppointmentRepository, AppointmentRepository>()
            .AddScoped<IAppointmentResultRepository, AppointmentResultRepository>();
    }
}
