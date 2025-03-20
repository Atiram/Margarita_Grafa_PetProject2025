using Clinic.DOMAIN;

namespace ClinicService.BLL.Services.Interfaces;
public interface INotificationHttpClient
{
    Task SendEventRequest(CreateEventMail createEventMail, CancellationToken cancellationToken);
}
