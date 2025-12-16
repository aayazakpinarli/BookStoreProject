using CORE.APP.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CORE.APP.Services.Authentication
{
    // Provides concrete implementations for token-based authentication operations
    public class TokenAuthService : ITokenAuthService
    {
        public TokenResponse GetTokenResponse(int userId, string userName, string[] userRoleNames, DateTime expiration,
            string securityKey, string issuer, string audience, string refreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", userId.ToString()), 
                new Claim(ClaimTypes.Name, userName)
            };
            foreach (var userRoleName in userRoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoleName));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(issuer, audience, claims, DateTime.Now, expiration, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var jwt = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new TokenResponse()
            {
                Token = $"{JwtBearerDefaults.AuthenticationScheme} {jwt}", 
                RefreshToken = refreshToken
            };
        }

        public string GetRefreshToken()
        {
            // Generate 32-byte refresh token.
            var bytes = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        public IEnumerable<Claim> GetClaims(string token, string securityKey)
        {
            // Remove the "Bearer" prefix if exists in the JWT
            token = token.StartsWith(JwtBearerDefaults.AuthenticationScheme) ?
                token.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length + 1) : token;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey
            };

            // Validate JWT
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            return securityToken is null ? null : principal.Claims;
        }
    }
}