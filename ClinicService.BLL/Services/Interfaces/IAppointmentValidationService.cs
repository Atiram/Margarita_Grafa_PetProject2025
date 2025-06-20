using ClinicService.BLL.Models.Requests;

namespace ClinicService.BLL.Services.Interfaces;
public interface IAppointmentValidationService
{
    Task ValidateAppointmentAvailabilityAsync(CreateAppointmentRequest request, CancellationToken cancellationToken);
}