using api.Models;
using System.IdentityModel.Tokens.Jwt;

namespace api.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(AppUser user);
    }
}
