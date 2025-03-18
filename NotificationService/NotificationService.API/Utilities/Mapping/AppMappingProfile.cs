using AutoMapper;
using NotificationService.API.ViewModels;
using NotificationService.DAL.Entities;

namespace NotificationService.API.Utilities.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<EventEntity, EventViewModel>();
    }
}
