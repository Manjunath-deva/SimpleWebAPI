using AutoMapper;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisWebAPI.Models;

namespace SimpleOtisWebAPI.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<WidgetModelDTO, WidgetDBDTO>().ReverseMap();
            CreateMap<UserPreferenceDTO, UserPreferenceDBDTO>().ReverseMap();
            CreateMap<DynamicMenuDTO, DynamicMenuDBDTO>().ReverseMap();
        }

    }
}
