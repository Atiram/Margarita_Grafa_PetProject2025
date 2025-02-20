using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces;
public interface IDoctorService
{
    Task<DoctorModel> GetById(Guid id);
    Task<DoctorModel> CreateAsync(DoctorModel model, CancellationToken ct);
    Task<DoctorModel> UpdateAsync(DoctorModel model);
    Task<bool> DeleteAsync(Guid id);
}
