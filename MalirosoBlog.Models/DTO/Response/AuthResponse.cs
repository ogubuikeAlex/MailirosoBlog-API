namespace MalirosoBlog.Models.DTO.Response
{
    public class UserResponseDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }

    public class JwtTokenResponse
    {
        public string Token { get; set; }
        public DateTime Issued { get; set; }
        public DateTime? Expires { get; set; }
    }

    public class LoginResponse
    {
        public JwtTokenResponse JwtToken { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }

    };
}
