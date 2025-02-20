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
    public async Task<PatientViewModel> Get(Guid id, CancellationToken cancellationToken)
    {
        var patientModel = await patientService.GetById(id, cancellationToken);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpPost]
    public async Task<PatientViewModel> Post(PatientViewModel item, CancellationToken cancellationToken)
    {
        var patientModel = await patientService.CreateAsync(mapper.Map<PatientModel>(item), cancellationToken);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpPut]
    public async Task<PatientViewModel> Put(PatientViewModel item, CancellationToken cancellationToken)
    {
        var patientModel = await patientService.UpdateAsync(mapper.Map<PatientModel>(item), cancellationToken);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await patientService.DeleteAsync(id, cancellationToken);
    }
}

