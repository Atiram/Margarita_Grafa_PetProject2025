using System.Text;
using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Newtonsoft.Json;
using NotificationService.DAL.Entities;
using NotificationService.DAL.Enums;

namespace ClinicService.BLL.Services;
public class AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, IMapper mapper) : IAppointmentService
{
    private const string Url = "https://localhost:7149/Event";
    private const string JsonContentType = "application/json";

    public async Task<AppointmentModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentEntity = await appointmentRepository.CreateAsync(mapper.Map<AppointmentEntity>(request), cancellationToken);

        var doctorEntity = await doctorRepository.GetByIdAsync((Guid)request.DoctorId, cancellationToken);

        string emailAddress = doctorEntity.Email;
        var metadata = new EventMetadata();
        metadata.SetMetadata("email", emailAddress);

        HttpClient httpClient = new HttpClient();
        EventEntity eventEntity = new EventEntity()
        {
            Type = EventType.Email,
            Metadata = emailAddress //metadata.Metadata
        };
        var content = new StringContent(JsonConvert.SerializeObject(eventEntity), Encoding.UTF8, JsonContentType);
        HttpResponseMessage response = await httpClient.PostAsync(Url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return mapper.Map<AppointmentModel>(appointmentEntity);
    }

    public async Task<AppointmentModel> UpdateAsync(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
        var appointmentEntity = mapper.Map(request, appointment);
        var updatedAppointmentEntity = await appointmentRepository.UpdateAsync(appointmentEntity, cancellationToken);

        return mapper.Map<AppointmentModel>(updatedAppointmentEntity);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return appointmentRepository.DeleteAsync(id, cancellationToken);
    }
}
