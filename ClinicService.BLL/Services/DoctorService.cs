using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Messages;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.DAL.Utilities.Pagination;

namespace ClinicService.BLL.Services;
public class DoctorService(IDoctorRepository doctorRepository, IMapper mapper) : IDoctorService
{
    public async Task<DoctorModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var doctorEntity = await doctorRepository.GetByIdAsync(id, cancellationToken);
        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<PagedResult<DoctorModel>> GetAll(GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken)
    {
        var doctorEntities = await doctorRepository.GetAllAsync(getAllDoctorsParams, cancellationToken);
        return mapper.Map<PagedResult<DoctorModel>>(doctorEntities);
    }

    public async Task<DoctorModel> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken)
    {
        if (request.FirstName.Length <= 3 || request.LastName.Length <= 3)
        {
            throw new ValidationException(ClinicNotificationMessages.validationExeptionMessage);
        }
        var doctorEntity = await doctorRepository.CreateAsync(mapper.Map<DoctorEntity>(request), cancellationToken);
        return mapper.Map<DoctorModel>(doctorEntity);
    }

    public async Task<DoctorModel> UpdateAsync(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        if (request.FirstName.Length <= 3 || request.LastName.Length <= 3)
        {
            throw new ValidationException(ClinicNotificationMessages.validationExeptionMessage);
        }
        var doctor = await doctorRepository.GetByIdAsync(request.Id, cancellationToken);
        var doctorEntity = mapper.Map(request, doctor);

        var updatedDoctorEntity = await doctorRepository.UpdateAsync(doctorEntity, cancellationToken);

        return mapper.Map<DoctorModel>(updatedDoctorEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await doctorRepository.DeleteAsync(id, cancellationToken);
    }
}
