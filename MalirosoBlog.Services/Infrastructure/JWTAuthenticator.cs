using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MalirosoBlog.Services.Infrastructure
{
    internal class JWTAuthenticator(JWTConfiguration jwtConfig) : IJWTAuthenticator
    {
        private readonly JWTConfiguration _jwtConfig = jwtConfig;

        public JwtTokenResponse GenerateJwtToken(ApplicationUser user, IEnumerable<Claim> additionalClaims, string expires = null)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            IdentityOptions _options = new();

            var claims = new List<Claim>
            {
                new("Id", user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(_options.ClaimsIdentity.UserIdClaimType, user.Id)
            };

            if (additionalClaims.Count() != 0)
            {
                claims.AddRange(additionalClaims);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = string.IsNullOrWhiteSpace(expires)
                    ? DateTime.Now.AddHours(double.Parse(_jwtConfig.Expires))
                    : DateTime.Now.AddMinutes(double.Parse(expires)),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new()
            {
                Token = jwtToken,
                Issued = DateTime.Now,
                Expires = tokenDescriptor.Expires
            };
        }
    }
}
