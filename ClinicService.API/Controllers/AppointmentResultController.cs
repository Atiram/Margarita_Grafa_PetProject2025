using AutoMapper;
using Clinic.Domain;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AppointmentResultController(IAppointmentResultService appointmentResultService, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<AppointmentResultViewModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var appointmentResultModel = await appointmentResultService.GetByIdAsync(id, cancellationToken);
        var appointmentResultViewModel = mapper.Map<AppointmentResultViewModel>(appointmentResultModel);

        return appointmentResultViewModel;
    }

    [HttpGet]
    public async Task<List<AppointmentResultViewModel>> GetAll(CancellationToken cancellationToken)
    {
        var appointmentResultModels = await appointmentResultService.GetAllAsync(cancellationToken);
        var appointmentResultViewModels = mapper.Map<List<AppointmentResultViewModel>>(appointmentResultModels);

        return appointmentResultViewModels;
    }

    [HttpPost]
    public async Task<AppointmentResultViewModel> Post(CreateAppointmentResultRequest request, CancellationToken cancellationToken)
    {
        var appointmentResultModel = await appointmentResultService.CreateAsync(request, cancellationToken);
        var appointmentResultViewModel = mapper.Map<AppointmentResultViewModel>(appointmentResultModel);

        return appointmentResultViewModel;
    }

    [HttpPut]
    public async Task<AppointmentResultViewModel> Put(UpdateAppointmentResultRequest request, CancellationToken cancellationToken)
    {
        var appointmentResultModel = await appointmentResultService.UpdateAsync(request, cancellationToken);
        var appointmentResultViewModel = mapper.Map<AppointmentResultViewModel>(appointmentResultModel);

        return appointmentResultViewModel;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var isDeleted = await appointmentResultService.DeleteAsync(id, cancellationToken);
        return isDeleted ? Ok() : NotFound(string.Format(NotificationMessages.NotFoundErrorMessage, id));
    }
}
