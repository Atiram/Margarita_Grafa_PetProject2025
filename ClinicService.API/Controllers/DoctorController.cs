using AutoMapper;
using ClinicService.API.ViewModels;
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
    private readonly IMapper _mapper;

    public DoctorController(ILogger<DoctorController> logger, IDoctorService doctorServise, IMapper mapper)
    {
        _logger = logger;
        _doctorService = doctorServise;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetDoctor")]
    public async Task<DoctorViewModel> Get(Guid id)
    {
        var doctorModel = await _doctorService.GetById(id);
        var doctorViewModel = _mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpPost(Name = "PostDoctor")]
    public async Task<DoctorViewModel> Post(DoctorViewModel item)
    {
        var doctorModel = await _doctorService.CreateAsync(_mapper.Map<DoctorModel>(item));
        var doctorViewModel = _mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpPut(Name = "PutDoctor")]
    public async Task<DoctorViewModel> Put(DoctorViewModel item)
    {
        var doctorModel = await _doctorService.UpdateAsync(_mapper.Map<DoctorModel>(item));
        var doctorViewModel = _mapper.Map<DoctorViewModel>(doctorModel);

        return doctorViewModel;
    }

    [HttpDelete(Name = "DeleteDoctor")]
    public async Task Delete(Guid id)
    {
        await _doctorService.DeleteAsync(id);
    }
}

