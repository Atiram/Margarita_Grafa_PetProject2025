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
        var doctorModel = mapper.Map<DoctorModel>(doctorEntity);

        return doctorModel;
    }

    public async Task<DoctorModel> CreateAsync(DoctorModel doctorModel)
    {
        var doctorEnt = mapper.Map<DoctorEntity>(doctorModel);

        var d = await doctorRepository.CreateAsync(doctorEnt);
        return doctorModel;
    }

    public async Task<DoctorModel> UpdateAsync(DoctorModel doctorModel)
    {
        var doctorEntity = mapper.Map<DoctorEntity>(doctorModel);
        var d = await doctorRepository.UpdateAsync(doctorEntity);

        return doctorModel;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await doctorRepository.DeleteAsync(id);
    }
}
