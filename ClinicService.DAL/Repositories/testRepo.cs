using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class TestRepository : GenericRepository<PatientEntity>, IPatientRepository
{
    public TestRepository(ClinicDbContext context) : base(context)
    {
    }
}
