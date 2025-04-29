using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentReminderController(IBackgroundWorkerService backgroundWorkerService) : ControllerBase
    {
        [HttpPost]
        public async Task SendReminder(CancellationToken cancellationToken)//<IActionResult>
        {
            await backgroundWorkerService.SendReminder(cancellationToken);

            //return OkResult;
        }
    }
}
