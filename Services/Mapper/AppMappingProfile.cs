using AutoMapper;
using Dal.Models;
using Dal.Models.DtoModels;

namespace Services.Mapper
{
    public class AppMappingProfile : Profile
    {   
        public AppMappingProfile()
        {
            CreateMap<ViewExperement, Experement>().ReverseMap();
        }
    }
}