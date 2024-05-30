using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public string? RefreshToken{ get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
