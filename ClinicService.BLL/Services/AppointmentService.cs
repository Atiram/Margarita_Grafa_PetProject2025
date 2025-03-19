using System.Text;
using AutoMapper;
using Clinic.DOMAIN;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Messages;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Newtonsoft.Json;

namespace ClinicService.BLL.Services;
public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IMapper mapper) : IAppointmentService
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
        var patientEntity = await patientRepository.GetByIdAsync((Guid)request.PatientId, cancellationToken);

        HttpClient httpClient = new HttpClient();
        CreateEventMail createEventMail = new CreateEventMail()
        {
            Email = emailAddress,
            Subject = NotificationMessages.emailSubject,
            Message = string.Format(NotificationMessages.emailMessageTemplate, appointmentEntity.Date, appointmentEntity.Slots, patientEntity?.FirstName, patientEntity?.LastName),
            CreatedAt = DateTime.UtcNow
        };

        var content = new StringContent(JsonConvert.SerializeObject(createEventMail), Encoding.UTF8, JsonContentType);
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
