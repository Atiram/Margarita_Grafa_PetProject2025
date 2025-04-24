using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Utilities.Pagination;

namespace ClinicService.BLL.Services.Interfaces;
public interface IDoctorService
{
    Task<DoctorModel?> GetById(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<DoctorModel>> GetAll(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken);
    Task<DoctorModel> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken);
    Task<DoctorModel> UpdateAsync(UpdateDoctorRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
