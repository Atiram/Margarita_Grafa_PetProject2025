using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class DoctorRepository : GenericRepository<DoctorEntity>, IDoctorRepository
{
    public DoctorRepository(ClinicDbContext context) : base(context)
    {
    }
}
