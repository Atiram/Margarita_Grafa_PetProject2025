using Clinic.DOMAIN;

namespace NotificationService.BLL.Services.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(CreateEventMail request);
}
