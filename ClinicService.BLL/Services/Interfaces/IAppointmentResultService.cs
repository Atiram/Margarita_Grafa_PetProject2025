using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;

namespace ClinicService.BLL.Services.Interfaces;
public interface IAppointmentResultService
{
    Task<AppointmentResultModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<AppointmentResultModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<AppointmentResultModel> CreateAsync(CreateAppointmentResultRequest request, CancellationToken cancellationToken);
    Task<AppointmentResultModel> UpdateAsync(UpdateAppointmentResultRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
