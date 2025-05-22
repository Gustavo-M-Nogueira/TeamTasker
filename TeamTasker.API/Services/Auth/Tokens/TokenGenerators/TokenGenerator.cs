using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TeamTasker.API.Services.Auth.Tokens.TokenGenerators
{
    public class TokenGenerator 
    {
        public string GenerateToken(
            string secretKey,
            string issuer,
            string audience,
            int expirationMinutes,
            IEnumerable<Claim>? claims = null) 
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expirationMinutes),
                credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
