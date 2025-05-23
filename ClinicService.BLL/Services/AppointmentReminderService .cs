﻿using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.RabbitMqProducer;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicService.BLL.Services;
public class AppointmentReminderService(
    IAppointmentService appointmentService,
    IRabbitMqService rabbitMqService,
    ILogger<AppointmentReminderService> logger
    ) : IAppointmentReminderService
{
    private readonly string emailSubject = NotificationMessages.ReminderAppointmentMessageSubject;
    private readonly string emailMessageTemplate = NotificationMessages.ReminderAppointmentMessageTemplate;

    public async Task SendRemindersJob(CancellationToken cancellationToken)
    {
        logger.LogInformation(NotificationMessages.HangfireJobStartedMessage);
        var now = DateTime.Now;
        var reminderTimeThreshold = now.AddHours(1);
        var upcomingAppointments = await GetUpcomingAppointments(now, cancellationToken);

        foreach (var appointment in upcomingAppointments)
        {
            DateTime appointmentTime = appointment.Date.ToDateTime(appointment.Slots);
            if (appointmentTime <= reminderTimeThreshold && appointmentTime > now)
            {
                SendReminderMessage(appointment);
            }
        }
        logger.LogInformation(NotificationMessages.HangfireJobCompletedMessage);
    }

    public async Task<List<AppointmentModel>> GetUpcomingAppointments(DateTime filterStartDate, CancellationToken cancellationToken)
    {
        return await appointmentService.GetFilteredAsync(filterStartDate, cancellationToken: cancellationToken);
    }

    public void SendReminderMessage(AppointmentModel appointment)
    {
        string doctorMessage = string.Format(emailMessageTemplate,
                appointment.Doctor.FirstName, appointment.Doctor.LastName,
                appointment.Patient.FirstName, appointment.Patient.LastName,
                appointment.Date, appointment.Slots);

        var doctorEmailEvent = new CreateEventMail()
        {
            Email = appointment.Doctor.Email,
            Subject = emailSubject,
            Message = doctorMessage,
            CreatedAt = DateTime.UtcNow,
        };

        rabbitMqService.SendMessage(doctorEmailEvent);
        var logMessage = string.Format(NotificationMessages.RabbitMQSettMessage, appointment.Doctor.Id, appointment.Id);
        logger.LogInformation(logMessage);
    }
}