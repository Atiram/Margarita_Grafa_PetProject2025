using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class PatientService(IPatientRepository patientRepository, IMapper mapper) : IPatientService
{
    public async Task<PatientModel> GetById(Guid id)
    {
        var patientEntity = await patientRepository.GetByIdAsync(id);

        return mapper.Map<PatientModel>(patientEntity);
    }

    public async Task<PatientModel> CreateAsync(PatientModel patientModel, CancellationToken ct)
    {
        var patientEntity = await patientRepository.CreateAsync(mapper.Map<PatientEntity>(patientModel));

        return mapper.Map<PatientModel>(patientEntity);
    }

    public async Task<PatientModel> UpdateAsync(PatientModel patientModel)
    {
        var patientEntity = mapper.Map<PatientEntity>(patientModel);
        var updatedPatientEntity = await patientRepository.UpdateAsync(patientEntity);

        return mapper.Map<PatientModel>(updatedPatientEntity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await patientRepository.DeleteAsync(id);
    }
}
