using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interface;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class PatientService(IPatientRepository patientRepository, IMapper mapper) : IPatientService
{
    public async Task<PatientModel> GetById(Guid id)
    {
        var patientEntity = await patientRepository.GetByIdAsync(id);
        var patientModel = mapper.Map<PatientModel>(patientEntity);

        return patientModel;
    }

    public async Task<PatientModel> CreateAsync(PatientModel patientModel)
    {
        var patientEntity = mapper.Map<PatientEntity>(patientModel);
        var p = await patientRepository.CreateAsync(patientEntity);

        return patientModel;
    }

    public async Task<PatientModel> UpdateAsync(PatientModel patientModel)
    {
        var patientEntity = mapper.Map<PatientEntity>(patientModel);
        var p = await patientRepository.UpdateAsync(patientEntity);

        return patientModel;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await patientRepository.DeleteAsync(id);
    }
}
