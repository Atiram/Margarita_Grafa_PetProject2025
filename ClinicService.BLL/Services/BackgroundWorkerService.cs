using ClinicService.BLL.RabbitMqProducer;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicService.BLL.Services;

public class BackgroundWorkerService(
    IServiceScopeFactory serviceScopeFactory,
    IAppointmentReminderService appointmentReminderService,
    ILogger<BackgroundWorkerService> logger
    ) : BackgroundService, IBackgroundWorkerService

{
    private const int CheckIntervalInSeconds = 60;

    public async Task SendReminder(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Appointment Reminder Service has started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();

                    var now = DateTime.Now;
                    var reminderTimeThreshold = now.AddHours(1);

                    var upcomingAppointments = await appointmentReminderService.GetUpcomingAppointments(appointmentService, reminderTimeThreshold, cancellationToken);

                    foreach (var appointment in upcomingAppointments)
                    {
                        DateTime appointmentTime = appointment.Date.ToDateTime(appointment.Slots);
                        if (appointmentTime <= reminderTimeThreshold && appointmentTime > now)
                        {
                            appointmentReminderService.SendReminderMessage(appointment);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(CheckIntervalInSeconds), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing appointment reminders.");
            }
        }

        logger.LogInformation("Appointment Reminder Service has completed its work.");
    }
}