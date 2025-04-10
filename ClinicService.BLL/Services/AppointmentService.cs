﻿using AutoMapper;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.RabbitMqProducer;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Messages;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    INotificationHttpClient notificationHttpClient,
    IMapper mapper,
    IRabbitMqService rabbitMqService
    ) : IAppointmentService
{
    public async Task<AppointmentModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.CreateAsync(mapper.Map<AppointmentEntity>(request), cancellationToken);

        var doctorEntity = await doctorRepository.GetByIdAsync((Guid)request.DoctorId, cancellationToken);
        string emailAddress = doctorEntity.Email;
        var patientEntity = await patientRepository.GetByIdAsync((Guid)request.PatientId, cancellationToken);

        CreateEventMail createEventMail = new CreateEventMail()
        {
            Email = emailAddress,
            Subject = ClinicNotificationMessages.emailSubject,
            Message = string.Format(ClinicNotificationMessages.emailMessageTemplate, appointmentEntity.Date, appointmentEntity.Slots, patientEntity?.FirstName, patientEntity?.LastName),
            CreatedAt = DateTime.UtcNow
        };
        rabbitMqService.SendMessage(createEventMail);
        
        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> UpdateAsync(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
        var appointmentEntity = mapper.Map(request, appointment);
        var updatedAppointmentEntity = await appointmentRepository.UpdateAsync(appointmentEntity, cancellationToken);

        return mapper.Map<AppointmentModel>(updatedAppointmentEntity);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return appointmentRepository.DeleteAsync(id, cancellationToken);
    }
}
