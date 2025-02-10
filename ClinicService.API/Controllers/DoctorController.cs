using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.DAL.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Margarita_Grafa_PetProject2025.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController : ControllerBase
{
    private readonly ILogger<DoctorController> _logger;
    //private readonly IDoctorRepository _doctorRepository;
    private readonly IDoctorService _doctorService;


    public DoctorController(ILogger<DoctorController> logger, IDoctorService doctorServise)
    {
        _logger = logger;
        _doctorService = doctorServise;
    }

    [HttpGet(Name = "GetDoctor")]
    public async Task<DoctorModel> Get(Guid id)
    {
        var doctorModel = await _doctorService.GetDoctorById(id);

        return doctorModel;
    }

    [HttpPost(Name = "PostDoctor")]
    public async Task<DoctorModel> Post(DoctorModel item)

    {
        //var doctor = new DoctorEntity();
        var doctor = new DoctorModel()
        {
            FirstName = "Willyyy",
            LastName = "Smith",
            DateOfBirth = new DateTime(1990, 01, 01),
            Email = "dd@ff",
            Specialization = "Terapy",
            Office = "New York",
            CareerStartYear = 2020,
            Status = DoctorStatus.SickDay
        };

        //var response = await _doctorRepository.CreateAsync(doctor);
        var response = await _doctorService.CreateDoctorAsync(doctor);

        return response;
    }

    [HttpPut(Name = "PutDoctor")]
    public async Task<DoctorModel> Put(DoctorModel item)
    {
        var doctor = new DoctorModel()
        {
            Id = Guid.Parse("9B343AAC-37D2-4096-88AB-08DD49CB5146"),
            FirstName = "Bobbyyyy",
            LastName = "Smith",
            DateOfBirth = new DateTime(1990, 01, 01),
            Email = "dd@fffffffff",
            Specialization = "Terapyyyyyyyy",
            Office = "New York",
            CareerStartYear = 2020,
            Status = DoctorStatus.SickDay
        };
        var response = await _doctorService.UpdateDoctorAsync(doctor);

        return response;
    }

    [HttpDelete(Name = "DeleteDoctor")]
    public async Task Delete(Guid id)
    {
        await _doctorService.DeleteDoctorAsync(id);
    }
}

