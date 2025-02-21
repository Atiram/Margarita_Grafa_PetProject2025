using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<DoctorEntity>
{
    Task<List<DoctorEntity>> GetAllAsync(
        bool isDescending,
        int pageNumber, 
        int pageSize,
        string s, 
        CancellationToken cancellationToken);
}