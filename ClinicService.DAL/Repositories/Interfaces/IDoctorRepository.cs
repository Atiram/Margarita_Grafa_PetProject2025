using ClinicService.DAL.Entities;
using ClinicService.DAL.Utilities.Pagination;

namespace ClinicService.DAL.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<DoctorEntity>
{
    Task<PagedResult<DoctorEntity>> GetAllAsync(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken);
}