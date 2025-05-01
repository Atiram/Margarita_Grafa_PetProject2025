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

    public async new Task<List<AppointmentEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentEntity>()
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .AsNoTracking()                 
                 .ToListAsync(cancellationToken);
    }

    public async Task<List<AppointmentEntity>> GetSortedAsync(CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentEntity>()
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .AsNoTracking()
                 .Where(a => a.Date >= DateOnly.FromDateTime(DateTime.Now) &&
                             a.Slots > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
                 .OrderBy(a => a.Date)
                 .ThenBy(a => a.Slots)
                 .ToListAsync(cancellationToken);
    }
}