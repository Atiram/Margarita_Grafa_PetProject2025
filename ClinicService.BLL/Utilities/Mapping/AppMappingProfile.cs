using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<DoctorEntity, DoctorModel>().ReverseMap();

        CreateMap<PatientEntity, PatientModel>().ReverseMap();

        CreateMap<AppointmentEntity, AppointmentModel>().ReverseMap();

        CreateMap<CreateAppointmentRequest, AppointmentEntity>();
    }
}
