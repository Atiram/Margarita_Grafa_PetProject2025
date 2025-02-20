using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class PatientService(IPatientRepository patientRepository, IMapper mapper) : IPatientService
{
    public async Task<PatientModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var patientEntity = await patientRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<PatientModel>(patientEntity);
    }

    public async Task<PatientModel> CreateAsync(PatientModel patientModel, CancellationToken cancellationToken)
    {
        var patientEntity = await patientRepository.CreateAsync(mapper.Map<PatientEntity>(patientModel), cancellationToken);

        return mapper.Map<PatientModel>(patientEntity);
    }

    public async Task<PatientModel> UpdateAsync(PatientModel patientModel, CancellationToken cancellationToken)
    {
        var patientEntity = mapper.Map<PatientEntity>(patientModel);
        var updatedPatientEntity = await patientRepository.UpdateAsync(patientEntity, cancellationToken);

        return mapper.Map<PatientModel>(updatedPatientEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await patientRepository.DeleteAsync(id, cancellationToken);
    }
}
