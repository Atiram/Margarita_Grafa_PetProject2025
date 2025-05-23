﻿using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.API.Controllers;
[Route("[controller]")]
[ApiController]
[EnableCors("AllowReactApp")]
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
    public async Task<PatientViewModel> Post(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        var patientModel = await patientService.CreateAsync(request, cancellationToken);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpPut]
    public async Task<PatientViewModel> Put(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        var patientModel = await patientService.UpdateAsync(request, cancellationToken);
        var patientViewModel = mapper.Map<PatientViewModel>(patientModel);

        return patientViewModel;
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await patientService.DeleteAsync(id, cancellationToken);
    }
}

