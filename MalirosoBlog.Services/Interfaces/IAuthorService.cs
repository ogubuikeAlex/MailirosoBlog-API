using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;

namespace MalirosoBlog.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorResponse> Get(string id);
        Task<AuthorResponse> GetByUserId(string id);
        Task<AuthorResponse> Create(CreateAuthorDTO request);
        Task<AuthorResponse> Delete(string id);
        Task<PagedResponse<AuthorResponse>> Get(AuthorRequestParameter request);
    }
}
