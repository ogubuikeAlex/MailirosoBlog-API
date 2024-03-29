using Microsoft.AspNetCore.Authorization;

namespace MalirosoBlog.Services.Infrastructure
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public int Success { get; set; }
    }
}
