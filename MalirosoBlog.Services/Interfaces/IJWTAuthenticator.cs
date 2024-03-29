using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;
using System.Security.Claims;

namespace MalirosoBlog.Services.Interfaces
{
    public interface IJWTAuthenticator
    {
        JwtTokenResponse GenerateJwtToken(ApplicationUser user, IEnumerable<Claim> additionalClaims, string expires = null);

    }
}
