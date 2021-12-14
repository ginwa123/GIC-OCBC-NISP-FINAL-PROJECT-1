using Microsoft.AspNetCore.Identity;
using Payment.Models;
using System.Threading.Tasks;

namespace Payment.Services.Auth
{
    public interface IAuthService
    {

        public Task<AuthResponse> GenerateJwToken(IdentityUser user);

        public Task<AuthResponse> RefreshJwtToken(TokenRequest token);

        public Task<AuthResponse> CheckJwtToken(TokenRequest tokenRequest);

        public UserManager<IdentityUser> UserManager();



    }
}
