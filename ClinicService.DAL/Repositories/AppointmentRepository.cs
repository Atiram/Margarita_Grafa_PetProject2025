using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class AppointmentRepository(ClinicDbContext context)
    : GenericRepository<AppointmentEntity>(context), IAppointmentRepository
{
    public new ValueTask<AppointmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = context.Set<AppointmentEntity>()
             .Include(a => a.Doctor)
             .Include(a => a.Patient)
             .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return new ValueTask<AppointmentEntity?>(task);
    }
}
