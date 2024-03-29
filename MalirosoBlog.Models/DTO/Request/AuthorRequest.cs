using System.ComponentModel.DataAnnotations;

namespace MalirosoBlog.Models.DTO.Request
{
    public class CreateAuthorDTO
    {
        [Required]
        public string UserId { get; set; }
    }

    public class AuthorRequestParameter : PagedRequestParameters
    {
        public AuthorRequestParameter()
        {
            OrderBy = "";
        }
    }
}
