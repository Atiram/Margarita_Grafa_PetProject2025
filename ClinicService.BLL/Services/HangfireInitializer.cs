using ClinicService.BLL.Services.Interfaces;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClinicService.BLL.Services;

public class HangfireInitializer(IServiceProvider _serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var appointmentReminderService = scope.ServiceProvider.GetRequiredService<IAppointmentReminderService>();

            RecurringJob.AddOrUpdate("SendAppointmentReminders", () => SendRemindersJobScoped(), Cron.Hourly());
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void SendRemindersJobScoped()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var appointmentReminderService = scope.ServiceProvider.GetRequiredService<IAppointmentReminderService>();
            appointmentReminderService.SendRemindersJob(CancellationToken.None).Wait();
        }
    }
}