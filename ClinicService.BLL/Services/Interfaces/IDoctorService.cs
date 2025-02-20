using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces;
public interface IDoctorService
{
    Task<DoctorModel> GetById(Guid id, CancellationToken cancellationToken);
    Task<DoctorModel> CreateAsync(DoctorModel model, CancellationToken cancellationToken);
    Task<DoctorModel> UpdateAsync(DoctorModel model, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
