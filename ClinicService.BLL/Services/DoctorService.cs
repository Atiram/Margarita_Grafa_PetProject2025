using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository) : IDoctorService
{
    public async Task<DoctorModel> GetDoctorById(Guid id)
    {
        var doctor = await doctorRepository.GetByIdAsync(id);
        var doctorModel = new DoctorModel()
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            MiddleName = doctor.MiddleName,
            DateOfBirth = doctor.DateOfBirth,
            Email = doctor.Email,
            Specialization = doctor.Specialization,
            Office = doctor.Office,
            CareerStartYear = doctor.CareerStartYear,
            Status = doctor.Status
        };
        return doctorModel;
    }

    public async Task<DoctorModel> CreateDoctorAsync(DoctorModel doctorModel)
    {
        var doctorEntity = new DoctorEntity()
        {
            Id = doctorModel.Id,
            FirstName = doctorModel.FirstName,
            LastName = doctorModel.LastName,
            MiddleName = doctorModel.MiddleName,
            DateOfBirth = doctorModel.DateOfBirth,
            Email = doctorModel.Email,
            Specialization = doctorModel.Specialization,
            Office = doctorModel.Office,
            CareerStartYear = doctorModel.CareerStartYear,
            Status = doctorModel.Status
        };
        var d = await doctorRepository.CreateAsync(doctorEntity);
        return doctorModel;
    }

    public async Task<DoctorModel> UpdateDoctorAsync(DoctorModel doctorModel)
    {
        var doctorEntity = new DoctorEntity()
        {
            Id = doctorModel.Id,
            FirstName = doctorModel.FirstName,
            LastName = doctorModel.LastName,
            MiddleName = doctorModel.MiddleName,
            DateOfBirth = doctorModel.DateOfBirth,
            Email = doctorModel.Email,
            Specialization = doctorModel.Specialization,
            Office = doctorModel.Office,
            CareerStartYear = doctorModel.CareerStartYear,
            Status = doctorModel.Status
        };
        var d = await doctorRepository.UpdateAsync(doctorEntity);

        return doctorModel;
    }

    public async Task<bool> DeleteDoctorAsync(Guid id)
    {
        return await doctorRepository.DeleteAsync(id);
    }
}
