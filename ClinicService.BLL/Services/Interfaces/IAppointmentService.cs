using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;

namespace ClinicService.BLL.Services.Interfaces;
public interface IAppointmentService
{
    Task<AppointmentModel> GetById(Guid id, CancellationToken cancellationToken);
    Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken);
    Task<AppointmentModel> UpdateAsync(UpdateAppointmentRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
