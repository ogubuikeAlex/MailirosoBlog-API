using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MalirosoBlog.Models.DTO.Request
{
    internal class AuthRequest
    {
    }

    public class CreateUserDTO
    {
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
