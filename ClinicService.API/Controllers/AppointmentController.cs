using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AppointmentController(IAppointmentService appointmentService, IMapper mapper) : ControllerBase
{

    [HttpGet]
    public async Task<AppointmentViewModel> Get(Guid id)
    {
        var appointmentModel = await appointmentService.GetById(id);
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpPost]
    public async Task<AppointmentViewModel> Post(AppointmentViewModel item)
    {
        var appointmentModel = await appointmentService.CreateAsync(mapper.Map<AppointmentModel>(item));
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpPut]
    public async Task<AppointmentViewModel> Put(AppointmentViewModel item)
    {
        var appointmentModel = await appointmentService.UpdateAsync(mapper.Map<AppointmentModel>(item));
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await appointmentService.DeleteAsync(id);
    }
}

