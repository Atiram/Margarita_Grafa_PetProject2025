using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using NotificationService.BLL.Models;
using NotificationService.BLL.Services.Interfaces;

namespace NotificationService.BLL.Services;
public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(CreateEventMail request)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings").Get<EmailConfiguration>();
        var emailMessage = PrepareMailMessage(request, smtpSettings);

        using (var client = ConfigureSmtpClient(smtpSettings))
        {
            await client.SendMailAsync(emailMessage);
        }
    }

    private SmtpClient ConfigureSmtpClient(EmailConfiguration smtpSettings)
    {
        var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port);
        client.Credentials = new NetworkCredential(smtpSettings.SenderEmail, smtpSettings.Password);
        client.EnableSsl = smtpSettings.Ssl;

        return client;
    }

    private MailMessage PrepareMailMessage(CreateEventMail request, EmailConfiguration smtpSettings)
    {
        var emailMessage = new MailMessage();

        emailMessage.From = new MailAddress(smtpSettings.SenderEmail, smtpSettings.SenderName);
        emailMessage.To.Add(new MailAddress(request.Email));
        emailMessage.Subject = request.Subject;
        emailMessage.IsBodyHtml = true;
        emailMessage.Body = request.Message;

        return emailMessage;
    }
}
