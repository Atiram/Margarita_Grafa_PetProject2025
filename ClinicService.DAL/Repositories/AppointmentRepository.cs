using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class AppointmentRepository : GenericRepository<AppointmentEntity>, IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    public AppointmentRepository(ClinicDbContext context) : base(context)
    {
        _context = context;
    }
    public new async Task<AppointmentEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<AppointmentEntity>()
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
