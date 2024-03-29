using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Services.Infrastructure;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MalirosoBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Authorization Based Operations")]
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService _authService = authService;

        [HttpPost(Name = "Login-User")]
        [SwaggerOperation(Summary = "Login In A User")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Logon Successful", Type = typeof(LoginResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "No record found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAuthor(LoginUserDTO request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPost("signup", Name = "Sign-Author")]
        [SwaggerOperation(Summary = "Sign Up To The App")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User Signed Up", Type = typeof(UserResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "User Already Exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAuthor(CreateUserDTO request)
        {
            return Ok(await _authService.SignUp(request));
        }
    }
}
