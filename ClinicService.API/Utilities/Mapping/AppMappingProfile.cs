using AutoMapper;
using ClinicService.API.ViewModels;
using ClinicService.BLL.Models;

namespace ClinicService.API.Utilities.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<DoctorModel, DoctorViewModel>().ReverseMap();

        CreateMap<PatientModel, PatientViewModel>().ReverseMap();

        CreateMap<AppointmentModel, AppointmentViewModel>().ReverseMap();

        CreateMap<AppointmentResultModel, AppointmentResultViewModel>().ReverseMap();
    }
}
