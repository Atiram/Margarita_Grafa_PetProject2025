using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Utilities.Pagination;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowReactApp")]
public class DoctorController(IDoctorService doctorService, IMapper mapper) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<DoctorViewModel> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var doctorModel = await doctorService.GetById(id, cancellationToken);
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpGet]
    public async Task<PagedResult<DoctorViewModel>> GetAll([FromQuery] GetAllDoctorsParams getAllDoctorsParams, CancellationToken cancellationToken)
    {
        var doctorModels = await doctorService.GetAll(getAllDoctorsParams, cancellationToken);
        var doctorViewModels = mapper.Map<PagedResult<DoctorViewModel>>(doctorModels);

        return doctorViewModels;
    }

    [HttpPost]
    public async Task<DoctorViewModel> Post(CreateDoctorRequest request, CancellationToken cancellationToken)
    {
        var doctorModel = await doctorService.CreateAsync(request, cancellationToken);
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpPut]
    public async Task<DoctorViewModel> Put(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        var doctorModel = await doctorService.UpdateAsync(request, cancellationToken);
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await doctorService.DeleteAsync(id, cancellationToken);
    }
}

