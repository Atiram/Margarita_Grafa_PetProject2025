using AutoMapper;
using OfficeService.BLL.Models;
using OfficeService.BLL.Models.Requests;
using OfficeService.DAL.Entities;

namespace OfficeService.BLL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<OfficeEntity, OfficeModel>().ReverseMap();

        CreateMap<CreateOfficeRequest, OfficeEntity>();

        CreateMap<UpdateOfficeRequest, OfficeEntity>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
