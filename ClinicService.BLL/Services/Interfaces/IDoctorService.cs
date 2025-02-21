using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces;
public interface IDoctorService
{
    Task<DoctorModel> GetById(Guid id, CancellationToken cancellationToken);
    Task<List<DoctorModel>> GetAll(bool isDescending, int pageNumber, int pageSize, string s, CancellationToken cancellationToken);
    Task<DoctorModel> CreateAsync(DoctorModel model, CancellationToken cancellationToken);
    Task<DoctorModel> UpdateAsync(DoctorModel model, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
