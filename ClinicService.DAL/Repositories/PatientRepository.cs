using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class PatientRepository : GenericRepository<PatientEntity>, IPatientRepository
{
    public PatientRepository(ClinicDbContext context) : base(context)
    {
    }
}
