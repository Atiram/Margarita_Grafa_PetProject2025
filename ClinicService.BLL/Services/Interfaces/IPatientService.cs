using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces;
public interface IPatientService
{
    Task<PatientModel> GetById(Guid id, CancellationToken cancellationToken);
    Task<PatientModel> CreateAsync(PatientModel model, CancellationToken cancellationToken);
    Task<PatientModel> UpdateAsync(PatientModel model, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
