using System.ComponentModel.DataAnnotations;
using Clinic.Domain;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicService.BLL.Services;
public class AppointmentValidationService(
    IAppointmentRepository appointmentRepository,
    ILogger<AppointmentValidationService> logger
    ) : IAppointmentValidationService
{
    public async Task ValidateAppointmentAvailabilityAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {

        var isDoctorBooked = await appointmentRepository.HasDoctorAppointmentAtTimeAsync(request.DoctorId, request.Date, request.Slots, cancellationToken);
        if (isDoctorBooked)
        {
            var message = string.Format(NotificationMessages.DoctorAlreadyBookedErrorMessage, request.DoctorId, request.Date, request.Slots);
            logger.LogWarning(message);
            throw new ValidationException(message);
        }
        var isPatientBooked = await appointmentRepository.HasPatientAppointmentAtTimeAsync(
        request.PatientId, request.Date, request.Slots, cancellationToken);

        if (isPatientBooked)
        {
            var message = string.Format(NotificationMessages.PatientAlreadyBookedErrorMessage, request.PatientId, request.Date, request.Slots);
            logger.LogWarning(message);
            throw new ValidationException(message);
        }
    }
}