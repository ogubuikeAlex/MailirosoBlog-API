using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Services.Implementation;
using MalirosoBlog.Services.Infrastructure;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MalirosoBlog.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [SwaggerTag("Author Based Operations")]
    public class AuthorController(IAuthorService authorService) : BaseController
    {
        private readonly IAuthorService _authorService = authorService;

        [HttpGet("{id}", Name = "Get-Author")]
        [SwaggerOperation(Summary = "Get An Author By Their Id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Author Retrieved", Type = typeof(AuthorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAuthor(string id)
        {
            return Ok(await _authorService.Get(id));
        }

        [HttpGet(Name = "Get-All-Authors")]
        [SwaggerOperation(Summary = "Get All Authors")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Authors Retrieved", Type = typeof(PagedList<AuthorResponse>))]        
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GellAllAuthors([FromQuery]AuthorRequestParameter request)
        {
            return Ok(await _authorService.Get(request));
        }

        [HttpPut("{id}", Name = "Delete-Author")]
        [SwaggerOperation(Summary = "Soft Delete An Author")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Author Soft Deleted", Type = typeof(AuthorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> SoftDeleteAuthor(string id)
        {
            return Ok(await _authorService.Delete(id));
        }
    }
}
