using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<AppointmentEntity>
{
    new ValueTask<AppointmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    new Task<List<AppointmentEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<List<AppointmentEntity>> GetSortedAsync(CancellationToken cancellationToken);
}