using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;

namespace ClinicService.BLL.Services.Interfaces;
public interface IPatientService
{
    Task<PatientModel> GetById(Guid id, CancellationToken cancellationToken);
    Task<PatientModel> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken);
    Task<PatientModel> UpdateAsync(UpdatePatientRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
