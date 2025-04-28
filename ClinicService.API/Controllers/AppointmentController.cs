using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;
[Route("[controller]")]
[ApiController]
[EnableCors("AllowReactApp")]
public class AppointmentController(IAppointmentService appointmentService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<List<AppointmentViewModel>> GetAll(CancellationToken cancellationToken)
    {
        var appointmentModels = await appointmentService.GetAllAsync(cancellationToken);
        var appointmentViewModels = mapper.Map<List<AppointmentViewModel>>(appointmentModels);

        return appointmentViewModels;
    }

    [HttpGet("{id}")]
    public async Task<AppointmentViewModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var appointmentModel = await appointmentService.GetById(id, cancellationToken);
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpPost]
    public async Task<AppointmentViewModel> Post(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentModel = await appointmentService.CreateAsync(request, cancellationToken);
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpPut]
    public async Task<AppointmentViewModel> Put(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentModel = await appointmentService.UpdateAsync(request, cancellationToken);
        var appointmentViewModel = mapper.Map<AppointmentViewModel>(appointmentModel);

        return appointmentViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await appointmentService.DeleteAsync(id, cancellationToken);
    }
}
