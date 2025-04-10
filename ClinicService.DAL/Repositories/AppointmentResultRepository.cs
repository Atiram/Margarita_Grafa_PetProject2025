using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;
public class AppointmentResultRepository(ClinicDbContext context) : GenericRepository<AppointmentResultEntity>(context), IAppointmentResultRepository
{
    public new ValueTask<AppointmentResultEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = context.Set<AppointmentResultEntity>()
          .Include(a => a.Appointment)
          .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return new ValueTask<AppointmentResultEntity?>(task);
    }
    public Task<List<AppointmentResultEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return context.Set<AppointmentResultEntity>()
            .Include(a => a.Appointment)
            .ToListAsync(cancellationToken);
    }
}
