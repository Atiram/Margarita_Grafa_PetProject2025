using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Margarita_Grafa_PetProject2025.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorRepository _doctorRepository;


        public DoctorController(ILogger<DoctorController> logger, IDoctorRepository doctorRepository)
        {
            _logger = logger;
            _doctorRepository = doctorRepository;
        }

        [HttpGet(Name = "GetDoctor")]
        public async Task<DoctorEntity> Get(int id)
        {
            return new DoctorEntity();
        }

        [HttpPost(Name = "PostDoctor")]
        public async Task<DoctorEntity> Post(DoctorEntity item)

        {
            var doctor = new DoctorEntity();

            var response = await _doctorRepository.CreateAsync(doctor);

            return response;
        }
    }
}
