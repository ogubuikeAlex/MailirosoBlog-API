using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Services.Extensions;
using MalirosoBlog.Services.Infrastructure;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MalirosoBlog.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AuthorsOnly")]
    [ApiController]
    [SwaggerTag("Blog Post operations")]
    public class BlogPostController(IBlogPostService blogPostService) : BaseController
    {
        private readonly IBlogPostService _blogPostService = blogPostService;

        [AllowAnonymous]
        [HttpGet("{id}", Name = "Get-Blog-Post")]
        [SwaggerOperation(Summary = "Get A Blog Post By Its Id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Blog Post Retrieved", Type = typeof(BlogPostResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetBlogPost(long id)
        {
            return Ok(await _blogPostService.Get(id));
        }

        [AllowAnonymous]
        [HttpGet(Name = "Get-All-BlogPosts")]
        [SwaggerOperation(Summary = "Get A Paginated List of Blog Posts")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Blog Posts Retrieved", Type = typeof(PagedResponse<BlogPostResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetBlogPosts([FromQuery] BlogPostRequestParameter request)
        {
            return Ok(await _blogPostService.Get(request));
        }

        [HttpPut("{id}", Name = "Update-Blog")]
        [SwaggerOperation(Summary = "Update The Title and Content Of A BlogPost")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Blog Post Updated", Type = typeof(BlogPostResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateBlogPost(long id, UpdateBlogPostDTO request)
        {
            return Ok(await _blogPostService.Update(id, request));
        }

        [HttpPost(Name = "Create-Blog")]
        [SwaggerOperation(Summary = "Create A Blog Post")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Blog Post Created", Type = typeof(BlogPostResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Blog Post Already Exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostDTO request)
        {
            return Ok(await _blogPostService.Create(request));
        }

        [HttpPut("delete/{id}", Name = "Delete-Blog")]
        [SwaggerOperation(Summary = "Soft Delete A Blog Post")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Blog Post Soft Deleted", Type = typeof(BlogPostResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> SoftDeleteBlogPost(long id)
        {
            return Ok(await _blogPostService.Delete(id));
        }
    }
}
