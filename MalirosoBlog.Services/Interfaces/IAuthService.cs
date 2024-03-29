using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;

namespace MalirosoBlog.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> SignUp(CreateUserDTO request);

        Task<LoginResponse> Login(LoginUserDTO request);
    }
}
