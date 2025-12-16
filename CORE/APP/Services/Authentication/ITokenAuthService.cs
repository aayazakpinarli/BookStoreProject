using CORE.APP.Models.Authentication;
using System.Security.Claims; 

namespace CORE.APP.Services.Authentication
{
    // Provides token-based authentication operations, including access token (JWT) and refresh token generation and claim extraction
    public interface ITokenAuthService
    {
        public TokenResponse GetTokenResponse(int userId, string userName, string[] userRoleNames, DateTime expiration,
            string securityKey, string issuer, string audience, string refreshToken); 

        public string GetRefreshToken();

        public IEnumerable<Claim> GetClaims(string token, string securityKey);
    }
}