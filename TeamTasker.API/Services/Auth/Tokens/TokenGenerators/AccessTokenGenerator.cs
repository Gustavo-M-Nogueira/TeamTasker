using System.Security.Claims;
using TeamTasker.API.Dtos;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Auth.Tokens.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthConfig _authConfig;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthConfig authConfig, TokenGenerator tokenGenerator)
        {
            _authConfig = authConfig;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Role, user.Role),
            };

            return _tokenGenerator.GenerateToken(
                _authConfig.AccessTokenSecret,
                _authConfig.Issuer,
                _authConfig.Audience,
                _authConfig.AccessTokenExpirationMinutes,
                claims
            );
        }
    }
}
