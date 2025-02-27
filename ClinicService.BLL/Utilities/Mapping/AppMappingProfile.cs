using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Utilities.Pagination;

namespace ClinicService.BLL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<DoctorEntity, DoctorModel>().ReverseMap();

        CreateMap<CreateDoctorRequest, DoctorEntity>();

        CreateMap<UpdateDoctorRequest, DoctorEntity>();

        CreateMap<PatientEntity, PatientModel>().ReverseMap();

        CreateMap<CreatePatientRequest, PatientEntity>();

        CreateMap<UpdatePatientRequest, PatientEntity>();//.ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<AppointmentEntity, AppointmentModel>().ReverseMap();

        CreateMap<CreateAppointmentRequest, AppointmentEntity>();

        CreateMap<UpdateAppointmentRequest, AppointmentEntity>();

        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ForMember(nameof(PagedResult<object>.Results), opt => opt.MapFrom(nameof(PagedResult<object>.Results)));
    }
}
