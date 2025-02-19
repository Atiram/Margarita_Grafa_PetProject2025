using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository, IMapper mapper) : IDoctorService
{
    public async Task<DoctorModel> GetById(Guid id)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id);

        return mapper.Map<DoctorModel>(doctorEntity); ;
    }

    public async Task<DoctorModel> CreateAsync(DoctorModel doctorModel)
    {
        var doctorEntity = await doctorRepository.CreateAsync(mapper.Map<DoctorEntity>(doctorModel));
        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<DoctorModel> UpdateAsync(DoctorModel doctorModel)
    {
        var doctorEntity = mapper.Map<DoctorEntity>(doctorModel);
        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await doctorRepository.DeleteAsync(id);
    }
}
