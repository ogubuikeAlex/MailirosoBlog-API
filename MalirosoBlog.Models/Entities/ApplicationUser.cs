using Microsoft.AspNetCore.Identity;

namespace MalirosoBlog.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }       
        public string LastName { get; set; }       
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
