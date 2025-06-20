using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<AppointmentEntity>
{
    new ValueTask<AppointmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    new Task<List<AppointmentEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<List<AppointmentEntity>> GetFilteredAsync(DateTime filterStartDate, bool isDescending, CancellationToken cancellationToken);

    Task<bool> HasDoctorAppointmentAtTimeAsync(Guid doctorId, DateOnly date, TimeOnly slots, CancellationToken cancellationToken);

    Task<bool> HasPatientAppointmentAtTimeAsync(Guid patientId, DateOnly date, TimeOnly slots, CancellationToken cancellationToken);
}