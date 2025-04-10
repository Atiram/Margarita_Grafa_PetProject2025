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

        CreateMap<UpdateDoctorRequest, DoctorEntity>()
            .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom((src, upd) => upd.CreatedAt))
            .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom((src, upd) => upd.UpdatedAt));

        CreateMap<PatientEntity, PatientModel>().ReverseMap();

        CreateMap<CreatePatientRequest, PatientEntity>();

        CreateMap<UpdatePatientRequest, PatientEntity>()
            .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom((src, upd) => upd.CreatedAt))
            .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom((src, upd) => upd.UpdatedAt));

        CreateMap<AppointmentEntity, AppointmentModel>().ReverseMap();

        CreateMap<CreateAppointmentRequest, AppointmentEntity>();

        CreateMap<UpdateAppointmentRequest, AppointmentEntity>()
            .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom((src, upd) => upd.CreatedAt))
            .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom((src, upd) => upd.UpdatedAt));

        CreateMap<AppointmentResultEntity, AppointmentResultModel>().ReverseMap();

        CreateMap<CreateAppointmentResultRequest, AppointmentResultEntity>();

        CreateMap<UpdateAppointmentResultRequest, AppointmentResultEntity>()
            .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom((src, upd) => upd.CreatedAt))
            .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom((src, upd) => upd.UpdatedAt));


        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ForMember(nameof(PagedResult<object>.Results), opt => opt.MapFrom(nameof(PagedResult<object>.Results)));
    }
}
