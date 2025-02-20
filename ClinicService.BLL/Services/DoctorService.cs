using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository, IMapper mapper) : IDoctorService
{
    public async Task<DoctorModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<DoctorModel> CreateAsync(DoctorModel doctorModel, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.CreateAsync(mapper.Map<DoctorEntity>(doctorModel), cancellationToken);
        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<DoctorModel> UpdateAsync(DoctorModel doctorModel, CancellationToken cancellationToken)
    {
        var doctorEntity = mapper.Map<DoctorEntity>(doctorModel);
        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity, cancellationToken);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await doctorRepository.DeleteAsync(id, cancellationToken);
    }
}
