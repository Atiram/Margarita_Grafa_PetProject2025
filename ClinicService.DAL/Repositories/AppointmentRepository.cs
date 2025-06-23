using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Repositories;

public class AppointmentRepository(ClinicDbContext context)
  : GenericRepository<AppointmentEntity>(context), IAppointmentRepository
{
    private static readonly TimeSpan AppointmentDuration = TimeSpan.FromMinutes(30);
    public async new ValueTask<AppointmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentEntity>()
          .Include(a => a.Doctor)
          .Include(a => a.Patient)
          .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async new Task<List<AppointmentEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<AppointmentEntity>()
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);
    }

    public async Task<List<AppointmentEntity>> GetFilteredAsync(DateTime filterStartDate, bool isDescending, CancellationToken cancellationToken)
    {
        IQueryable<AppointmentEntity> query = context.Set<AppointmentEntity>()
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .AsNoTracking()
            .Where(a => a.Date == DateOnly.FromDateTime(DateTime.Now));

        query = isDescending ? query.OrderByDescending(a => a.Date).ThenByDescending(a => a.Slots)
            : query.OrderBy(a => a.Date).ThenBy(a => a.Slots);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> HasDoctorAppointmentAtTimeAsync(Guid doctorId, DateOnly date, TimeOnly slots, CancellationToken cancellationToken)
    {
        DateTime proposedAppointmentStart = date.ToDateTime(slots);
        DateTime proposedAppointmentEnd = proposedAppointmentStart.Add(AppointmentDuration);

        var appointmentsOnDate = await context.Set<AppointmentEntity>()
            .AsNoTracking()
            .Where(a => a.DoctorId == doctorId && a.Date == date)
            .ToListAsync(cancellationToken);

        return appointmentsOnDate.Any(existingAppointment =>
        {
            DateTime existingStartDateTime = existingAppointment.Date.ToDateTime(existingAppointment.Slots);
            DateTime existingEndDateTime = existingStartDateTime.Add(AppointmentDuration);

            return proposedAppointmentStart < existingEndDateTime && proposedAppointmentEnd > existingStartDateTime;
        });
    }

    public async Task<bool> HasPatientAppointmentAtTimeAsync(Guid patientId, DateOnly date, TimeOnly slots, CancellationToken cancellationToken)
    {
        DateTime proposedAppointmentStart = date.ToDateTime(slots);
        DateTime proposedAppointmentEnd = proposedAppointmentStart.Add(AppointmentDuration);

        var appointmentsOnDate = await context.Set<AppointmentEntity>()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId && a.Date == date)
            .ToListAsync(cancellationToken);

        return appointmentsOnDate.Any(existingAppointment =>
        {
            DateTime existingStartDateTime = existingAppointment.Date.ToDateTime(existingAppointment.Slots);
            DateTime existingEndDateTime = existingStartDateTime.Add(AppointmentDuration);

            return proposedAppointmentStart < existingEndDateTime && proposedAppointmentEnd > existingStartDateTime;
        });
    }
}