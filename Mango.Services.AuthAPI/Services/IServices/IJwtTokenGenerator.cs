using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles);
    }
}
