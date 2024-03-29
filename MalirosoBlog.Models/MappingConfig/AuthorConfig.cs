using AutoMapper;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;

namespace MalirosoBlog.Models.MappingConfig
{
    public class AuthorConfig : Profile
    {
        public AuthorConfig()
        {
            CreateMap<CreateAuthorDTO, Author>();

            CreateMap<AuthorResponse, Author>().ReverseMap()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))                
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Id));                
        }
    }
}
