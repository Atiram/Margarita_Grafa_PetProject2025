using AutoMapper;
using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;
using DocumentService.DAL.Entities;

namespace DocumentService.BBL.Utilities.Mapping;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<FileEntity, FileModel>().ReverseMap();
        CreateMap<CreateFileRequest, FileEntity>();
    }
}
