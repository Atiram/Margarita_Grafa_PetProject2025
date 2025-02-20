using ClinicService.DAL.Data;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.DAL.Repositories;

public class DoctorRepository(ClinicDbContext context) : GenericRepository<DoctorEntity>(context), IDoctorRepository;
