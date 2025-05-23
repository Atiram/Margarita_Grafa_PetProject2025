﻿using ClinicService.BLL.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ClinicService.BLL.Services;

public class BackgroundWorkerService(IAppointmentReminderService appointmentReminderService) : BackgroundService, IBackgroundWorkerService
{
    private const int CheckIntervalInSeconds = 30;

    public async Task SendReminder(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var reminderTimeThreshold = now.AddHours(1);

            var upcomingAppointments = await appointmentReminderService.GetUpcomingAppointments(now, stoppingToken);

            foreach (var appointment in upcomingAppointments)
            {
                DateTime appointmentTime = appointment.Date.ToDateTime(appointment.Slots);
                if (appointmentTime <= reminderTimeThreshold && appointmentTime > now)
                {
                    appointmentReminderService.SendReminderMessage(appointment);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(CheckIntervalInSeconds), stoppingToken);
        }
    }
}