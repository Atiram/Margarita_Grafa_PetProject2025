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

        CreateMap<PatientEntity, PatientModel>().ReverseMap();

        CreateMap<AppointmentEntity, AppointmentModel>().ReverseMap();

        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ForMember(nameof(PagedResult<object>.Results), opt => opt.MapFrom(nameof(PagedResult<object>.Results)));

        CreateMap<CreateAppointmentRequest, AppointmentEntity>();

        CreateMap<UpdateAppointmentRequest, AppointmentEntity>();
    }
}
