using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interface;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class PatientService(IPatientRepository patientRepository) : IPatientService
{
    public async Task<PatientModel> GetPatientById(Guid id)
    {
        var patient = await patientRepository.GetByIdAsync(id);
        var patientModel = new PatientModel()
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            MiddleName = patient.MiddleName,
            PhoneNumber = patient.PhoneNumber,
            DateOfBirth = patient.DateOfBirth,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt,
        };
        return patientModel;
    }

    public async Task<PatientModel> CreatePatientAsync(PatientModel patientModel)
    {
        var patientEntity = new PatientEntity()
        {
            Id = patientModel.Id,
            FirstName = patientModel.FirstName,
            LastName = patientModel.LastName,
            MiddleName = patientModel.MiddleName,
            PhoneNumber = patientModel.PhoneNumber,
            DateOfBirth = patientModel.DateOfBirth,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

        };
        var p = await patientRepository.CreateAsync(patientEntity);
        return patientModel;
    }

    public async Task<PatientModel> UpdatePatientAsync(PatientModel patientModel)
    {
        var patientEntity = new PatientEntity()
        {
            Id = patientModel.Id,
            FirstName = patientModel.FirstName,
            LastName = patientModel.LastName,
            MiddleName = patientModel.MiddleName,
            PhoneNumber = patientModel.PhoneNumber,
            DateOfBirth = patientModel.DateOfBirth,
            CreatedAt = patientModel.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
        };
        var p = await patientRepository.UpdateAsync(patientEntity);
        return patientModel;
    }

    public async Task<bool> DeletePatientAsync(Guid id)
    {
        return await patientRepository.DeleteAsync(id);
    }
}
