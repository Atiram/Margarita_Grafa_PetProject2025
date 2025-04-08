using AutoMapper;
using Azure.Core;
using Clinic.Domain;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class AppointmentResultService(IAppointmentResultRepository appointmentResultRepository, IMapper mapper) : IAppointmentResultService
{
    public async Task<AppointmentResultModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var appointmentResultEntity = await appointmentResultRepository.GetByIdAsync(id, cancellationToken);
        if (appointmentResultEntity == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        return mapper.Map<AppointmentResultModel>(appointmentResultEntity);
    }

    public async Task<List<AppointmentResultModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var appointmentResultEntities = await appointmentResultRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<AppointmentResultModel>>(appointmentResultEntities);
    }

    public async Task<AppointmentResultModel> CreateAsync(CreateAppointmentResultRequest request, CancellationToken cancellationToken)
    {
        var appointmentResultEntity = await appointmentResultRepository.CreateAsync(mapper.Map<AppointmentResultEntity>(request), cancellationToken);
        return mapper.Map<AppointmentResultModel>(appointmentResultEntity);
    }

    public async Task<AppointmentResultModel> UpdateAsync(UpdateAppointmentResultRequest request, CancellationToken cancellationToken)
    {

        var appointmentResult = await appointmentResultRepository.GetByIdAsync(request.Id, cancellationToken);
        if (appointmentResult == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, request.Id));
        }
        var appointmentResultEntity = mapper.Map(request, appointmentResult);

        var updatedAppointmentResultEntity = await appointmentResultRepository.UpdateAsync(appointmentResultEntity, cancellationToken);

        return mapper.Map<AppointmentResultModel>(updatedAppointmentResultEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await appointmentResultRepository.DeleteAsync(id, cancellationToken);
    }
}
