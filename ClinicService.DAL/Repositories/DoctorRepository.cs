using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;

namespace ClinicService.DAL.Repositories
{
    internal class DoctorRepository : GenericRepository<DoctorEntity>
    {
        public DoctorRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
