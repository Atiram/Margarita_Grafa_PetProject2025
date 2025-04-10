using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories.Interfaces;
public interface IAppointmentResultRepository : IGenericRepository<AppointmentResultEntity>
{
    Task<List<AppointmentResultEntity>> GetAllAsync(CancellationToken cancellationToken);
}
