using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.RabbitMqProducer; // Используем Producer из BLL
using ClinicService.BLL.Services.Interfaces; //может быть общим
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicService.BLL.Services;
public class AppointmentReminderService(
    IRabbitMqService rabbitMqService,
    ILogger<AppointmentReminderService> logger
    ) : IAppointmentReminderService
{
    private const int CheckIntervalInSeconds = 60;
    private readonly string _emailSubject = "Reminder about the upcoming appointment";
    private readonly string _emailMessageTemplate =
        "Dear {0} {1}, we remind you about the upcoming appointment {4} at {5} with {3} {2}";

    //public async Task SendReminder(CancellationToken cancellationToken)
    //{
    //    await ExecuteAsync(cancellationToken);
    //}

    //protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    //{
    //    logger.LogInformation("Appointment Reminder Service has started.");

    //    while (!cancellationToken.IsCancellationRequested)
    //    {
    //        try
    //        {
    //            using (var scope = serviceScopeFactory.CreateScope())
    //            {
    //                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();

    //                var now = DateTime.Now;
    //                var reminderTimeThreshold = now.AddHours(1);

    //                var upcomingAppointments = await GetUpcomingAppointments(appointmentService, reminderTimeThreshold, cancellationToken);

    //                foreach (var appointment in upcomingAppointments)
    //                {
    //                    DateTime appointmentTime = appointment.Date.ToDateTime(appointment.Slots);
    //                    if (appointmentTime <= reminderTimeThreshold && appointmentTime > now)
    //                    {
    //                        SendReminderMessage(appointment);
    //                    }
    //                }
    //            }

    //            await Task.Delay(TimeSpan.FromSeconds(CheckIntervalInSeconds), cancellationToken);
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.LogError(ex, "An error occurred while processing appointment reminders.");
    //        }
    //    }

    //    logger.LogInformation("Appointment Reminder Service has completed its work.");
    //}

    public async Task<List<AppointmentModel>> GetUpcomingAppointments(IAppointmentService appointmentService, DateTime reminderTimeThreshold, CancellationToken stoppingToken)
    {
        var allAppointments = await appointmentService.GetAllAsync(stoppingToken);
        return allAppointments
            .Where(a => a.Date.ToDateTime(a.Slots) > DateTime.Now)
            .OrderBy(a => a.Date)
            .ThenBy(a => a.Slots)
            .ToList();
    }

    public void SendReminderMessage(AppointmentModel appointment)
    {
        string doctorMessage = string.Format(_emailMessageTemplate,
                appointment.Doctor.FirstName, appointment.Doctor.LastName,
                appointment.Patient.FirstName, appointment.Patient.LastName,
                appointment.Date, appointment.Slots);

        var doctorEmailEvent = new CreateEventMail()
        {
            Email = appointment.Doctor.Email,
            Subject = _emailSubject,
            Message = doctorMessage,
            CreatedAt = DateTime.UtcNow,
        };

        rabbitMqService.SendMessage(doctorEmailEvent);
        logger.LogInformation($"Message sent to RabbitMQ for doctor {appointment.Doctor.Email} regarding appointment {appointment.Id}");

        string patientMessage = string.Format(_emailMessageTemplate,
                appointment.Patient.FirstName, appointment.Patient.LastName,
                appointment.Doctor.FirstName, appointment.Doctor.LastName,
                appointment.Date, appointment.Slots);
        var patientEmailEvent = new CreateEventMail()
        {
            Email = appointment.Patient.PhoneNumber, 
            Subject = _emailSubject,
            Message = patientMessage,
            CreatedAt = DateTime.UtcNow,
        };
        rabbitMqService.SendMessage(patientEmailEvent);
        logger.LogInformation($"Message sent to RabbitMQ for patient {appointment.Patient.PhoneNumber} regarding appointment {appointment.Id}");
    }
}