using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentReminderController(IAppointmentReminderService appointmentReminderService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendReminder(CancellationToken cancellationToken)
        {
            await appointmentReminderService.SendRemindersJob(cancellationToken);
            return Ok();
        }
    }
}
