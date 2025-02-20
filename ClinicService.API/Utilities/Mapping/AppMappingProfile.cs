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
        CreateMap<AppointmentModel, AppointmentViewModel>()
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Doctor.Id))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Patient.Id))
            .ReverseMap();

        //      CreateMap<DoctorModel, DoctorViewModel>()
        //.ReverseMap()
        //.ForMember(dest => dest.Appointments, opt => opt.Ignore()); 

        //      CreateMap<PatientModel, PatientViewModel>()
        //        .ReverseMap()
        //        .ForMember(dest => dest.Appointments, opt => opt.Ignore()); 

        //      CreateMap<AppointmentModel, AppointmentViewModel>()
        //        .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Doctor.Id))
        //        .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Patient.Id))
        //        .ReverseMap();
    }
}
