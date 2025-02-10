using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interface;
public interface IPatientService
{
    Task<PatientModel> GetPatientById(Guid id);
    Task<PatientModel> CreatePatientAsync(PatientModel patientModel);
    Task<PatientModel> UpdatePatientAsync(PatientModel patientModel);
    Task<bool> DeletePatientAsync(Guid id);
}
