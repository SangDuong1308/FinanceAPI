using api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(AppUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    }
}
