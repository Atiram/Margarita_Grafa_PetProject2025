using ClinicService.BLL.Models;

namespace ClinicService.BLL.Services.Interfaces
{
    public interface IAppointmentReminderService
    {
        Task SendRemindersJob(CancellationToken cancellationToken);

        Task<List<AppointmentModel>> GetUpcomingAppointments(DateTime reminderTimeThreshold, CancellationToken stoppingToken);

        void SendReminderMessage(AppointmentModel appointment);
    }
}
