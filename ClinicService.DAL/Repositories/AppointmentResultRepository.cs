using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;
public class AppointmentResultRepository(ClinicDbContext context) : GenericRepository<AppointmentResultEntity>(context), IAppointmentResultRepository
{
    public async new ValueTask<AppointmentResultEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentResultEntity>()
          .Include(a => a.Appointment)
          .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
    public async new Task<List<AppointmentResultEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentResultEntity>()
            .Include(a => a.Appointment)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
