using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;
[Route("[controller]")]
[ApiController]
public class PatientController(IPatientService patientService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<PatientViewModel> Get(Guid id)
    {
        var patientModel = await patientService.GetById(id);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpPost]
    public async Task<PatientViewModel> Post(PatientViewModel item)
    {
        var patientModel = await patientService.CreateAsync(mapper.Map<PatientModel>(item));
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpPut]
    public async Task<PatientViewModel> Put(PatientViewModel item)
    {
        var patientModel = await patientService.UpdateAsync(mapper.Map<PatientModel>(item));
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id)
    {
        await patientService.DeleteAsync(id);
    }
}

