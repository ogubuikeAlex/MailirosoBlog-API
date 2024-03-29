using AutoMapper;
using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MalirosoBlog.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IServiceFactory _serviceFactory;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IRepository<ApplicationUser> _userRepo;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IServiceFactory serviceFactory, UserManager<ApplicationUser> userManager)
        {
            _serviceFactory = serviceFactory;

            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            _mapper = _serviceFactory.GetService<IMapper>();

            _userRepo = _unitOfWork.GetRepository<ApplicationUser>();

            _userManager = userManager;
        }
        public async Task<LoginResponse> Login(LoginUserDTO request)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Email.ToLower().Trim()) ?? throw new InvalidOperationException("Invalid Email or Password");

            if (!user.Active)
                throw new InvalidOperationException("Invalid Email Or Password");

            bool result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                throw new InvalidOperationException("Invalid Email Or Password");           

            var claims = (await _userManager.GetRolesAsync(user))?
                .Select( x => new Claim(ClaimTypes.Role, x)); 

            var userToken = await GetTokenAsync(user, claims);

            var response = _mapper.Map<LoginResponse>(user);

            response.JwtToken = userToken;

            return response;
        }

        private async Task<JwtTokenResponse> GetTokenAsync(ApplicationUser user, IEnumerable<Claim> roles, string expires = null)
        {
            IJWTAuthenticator authenticator = _serviceFactory.GetService<IJWTAuthenticator>();

            return authenticator.GenerateJwtToken(user, roles, expires);
        }

        public async Task<UserResponseDTO> SignUp(CreateUserDTO request)
        {
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                throw new InvalidOperationException($"User already exists with Email {request.Email}");

            ApplicationUser user = new()
            {
                Email = request.Email.ToLower(),
                UserName = request.Email.ToLower(),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Active = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create user: {(result.Errors.FirstOrDefault())?.Description}");
            }
           
            var authorService = _serviceFactory.GetService<IAuthorService>();

            var authorProfile = await authorService.Create(new() { UserId = user.Id});

            if(authorProfile is null)
            {
                await _userManager.DeleteAsync(user);

                throw new Exception("Signup Failed");
            }

            AddUserToRoleRequest userRole = new() { Email = user.Email, Role = "Author" };

            await _serviceFactory.GetService<IRoleService>().AddUserToRole(userRole);

            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}
