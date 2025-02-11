using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Margarita_Grafa_PetProject2025.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController : ControllerBase
{
    private readonly ILogger<DoctorController> _logger;
    private readonly IDoctorService _doctorService;

    public DoctorController(ILogger<DoctorController> logger, IDoctorService doctorServise)
    {
        _logger = logger;
        _doctorService = doctorServise;
    }

    [HttpGet(Name = "GetDoctor")]
    public async Task<DoctorModel> Get(Guid id)
    {
        var doctorModel = await _doctorService.GetById(id);

        return doctorModel;
    }

    [HttpPost(Name = "PostDoctor")]
    public async Task<DoctorModel> Post(DoctorModel item)

    {
        var doctor = new DoctorModel();
        var response = await _doctorService.CreateAsync(doctor);

        return response;
    }

    [HttpPut(Name = "PutDoctor")]
    public async Task<DoctorModel> Put(DoctorModel item)
    {
        var doctor = new DoctorModel();

        var response = await _doctorService.UpdateAsync(doctor);

        return response;
    }

    [HttpDelete(Name = "DeleteDoctor")]
    public async Task Delete(Guid id)
    {
        await _doctorService.DeleteAsync(id);
    }
}

