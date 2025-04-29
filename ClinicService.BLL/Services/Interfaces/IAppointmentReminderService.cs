using ClinicService.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicService.BLL.Services.Interfaces
{
    public interface IAppointmentReminderService
    {
        Task<List<AppointmentModel>> GetUpcomingAppointments(IAppointmentService appointmentService, DateTime reminderTimeThreshold, CancellationToken stoppingToken);
        
        void SendReminderMessage(AppointmentModel appointment);
    }
}
