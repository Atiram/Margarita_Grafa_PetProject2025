using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories
{
    internal class PatientRepository : GenericRepository<PatientEntity>
    {
        public PatientRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
