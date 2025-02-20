using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;

namespace ClinicService.BLL.Services;
public class AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper) : IAppointmentService
{
    public async Task<AppointmentModel> GetById(Guid id)
    {
        var appointmentEntity = await appointmentRepository.GetByIdAsync(id);
        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken ct)
    {
        var appointmentEntity = await appointmentRepository.CreateAsync(mapper.Map<AppointmentEntity>(request));
        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> UpdateAsync(AppointmentModel appointmentModel)
    {
        var appointmentEntity = mapper.Map<AppointmentEntity>(appointmentModel);
        var updatedAppointmentEntity = await appointmentRepository.UpdateAsync(appointmentEntity);

        return mapper.Map<AppointmentModel>(updatedAppointmentEntity);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return appointmentRepository.DeleteAsync(id);
    }
}
