using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;

namespace ClinicService.BLL.Services.Interfaces;
public interface IAppointmentService
{
    Task<AppointmentModel> GetById(Guid id);
    Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken ct);
    Task<AppointmentModel> UpdateAsync(AppointmentModel model);
    Task<bool> DeleteAsync(Guid id);
}
