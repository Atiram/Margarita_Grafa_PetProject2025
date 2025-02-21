using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper) : IAppointmentService
{
    public async Task<AppointmentModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.CreateAsync(mapper.Map<AppointmentEntity>(request), cancellationToken);

        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> UpdateAsync(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentEntity = mapper.Map<AppointmentEntity>(request);
        var updatedAppointmentEntity = await appointmentRepository.UpdateAsync(appointmentEntity, cancellationToken);

        return mapper.Map<AppointmentModel>(updatedAppointmentEntity);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return appointmentRepository.DeleteAsync(id, cancellationToken);
    }
}
