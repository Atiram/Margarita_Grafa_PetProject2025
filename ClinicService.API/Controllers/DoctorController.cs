using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Margarita_Grafa_PetProject2025.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController(ILogger<DoctorController> logger, IDoctorService doctorService, IMapper mapper) : ControllerBase
{

    [HttpGet(Name = "GetDoctor")]
    public async Task<DoctorViewModel> Get(Guid id)
    {
        var doctorModel = await doctorService.GetById(id);
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpPost(Name = "PostDoctor")]
    public async Task<DoctorViewModel> Post(DoctorViewModel item)
    {
        var doctorModel = await doctorService.CreateAsync(mapper.Map<DoctorModel>(item));
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpPut(Name = "PutDoctor")]
    public async Task<DoctorViewModel> Put(DoctorViewModel item)
    {
        var doctorModel = await doctorService.UpdateAsync(mapper.Map<DoctorModel>(item));
        var doctorViewModel = mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpDelete(Name = "DeleteDoctor")]
    public async Task Delete(Guid id)
    {
        await doctorService.DeleteAsync(id);
    }
}

