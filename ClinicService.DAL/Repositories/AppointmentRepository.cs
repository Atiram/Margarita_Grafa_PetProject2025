using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class AppointmentRepository : GenericRepository<AppointmentEntity>, IAppointmentRepository
{
    public AppointmentRepository(ClinicDbContext context) : base(context)
    {
    }
}
