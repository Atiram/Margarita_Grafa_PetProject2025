using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces;
public interface IPatientService
{
    Task<PatientModel> GetById(Guid id);
    Task<PatientModel> CreateAsync(PatientModel model, CancellationToken ct);
    Task<PatientModel> UpdateAsync(PatientModel model);
    Task<bool> DeleteAsync(Guid id);
}
