using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalirosoBlog.Models.DTO.Request
{
    internal class RoleRequest
    {
    }

    public class AddUserToRoleRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role Name cannot be empty")]
        [MinLength(2), MaxLength(50)]
        public string Role { get; set; }
    }
}
