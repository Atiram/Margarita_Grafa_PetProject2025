using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<DoctorEntity, DoctorModel>()
           .ForMember(dest => dest.Appointments, opt => opt.Ignore())
           .ForMember(dest => dest.Appointments, opt => opt.Ignore())
           .ReverseMap();

        CreateMap<PatientEntity, PatientModel>()
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<AppointmentEntity, AppointmentModel>()
           .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Doctor.Id))
           .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Patient.Id))
           .ReverseMap();
    }
}
