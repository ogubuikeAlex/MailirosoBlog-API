using MalirosoBlog.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MalirosoBlog.Models.DTO.Request
{
    public class CreateBlogPostDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }

    public class UpdateBlogPostDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }       
    }

    public class BlogPostRequestParameter : PagedRequestParameters
    {
        public BlogPostRequestParameter()
        {
            OrderBy = "Title";
        }
    }
}
