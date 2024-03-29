using AutoMapper;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;

namespace MalirosoBlog.Models.MappingConfig
{
    public class BlogPostConfig : Profile
    {
        public BlogPostConfig()
        {
            CreateMap<BlogPost, CreateBlogPostDTO>().ReverseMap();

            CreateMap<UpdateBlogPostDTO, BlogPost>().ReverseMap();

            CreateMap<BlogPost, BlogPostResponse>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.CreatedAt.ToUniversalTime()))
                .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstNameOfAuthor, opt => opt.MapFrom(src => src.Author.User.FirstName))
                .ForMember(dest => dest.LastNameOfAuthor, opt => opt.MapFrom(src => src.Author.User.LastName)) ;

            CreateMap<AuthorResponse, BlogPostResponse>()                
                .ForMember(dest => dest.FirstNameOfAuthor, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastNameOfAuthor, opt => opt.MapFrom(src => src.LastName)); ;
        }
    }
}
