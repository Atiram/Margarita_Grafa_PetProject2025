using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<AppointmentEntity>
{
    new Task<List<AppointmentEntity>> GetAllAsync(CancellationToken cancellationToken);
}