using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<DoctorEntity, DoctorModel>().ReverseMap();
    }
}
