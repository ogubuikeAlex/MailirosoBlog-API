using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;

namespace MalirosoBlog.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<BlogPostResponse> Get(long id);
        Task<PagedResponse<BlogPostResponse>> Get(BlogPostRequestParameter request);
        Task<BlogPostResponse> Update(long id, UpdateBlogPostDTO request);
        Task<BlogPostResponse> Create(CreateBlogPostDTO request);
        Task<BlogPostResponse> Delete(long id);
    }
}
