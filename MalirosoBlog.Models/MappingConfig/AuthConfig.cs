using AutoMapper;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;

namespace MalirosoBlog.Models.MappingConfig
{
    public class AuthConfig : Profile
    {
        public AuthConfig()
        {
            CreateMap<ApplicationUser, LoginResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ApplicationUser, UserResponseDTO>();
                
        }
    }
}
