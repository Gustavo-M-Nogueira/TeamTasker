using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Auth.Tokens.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly AuthConfig _authConfig;

        public RefreshTokenValidator(AuthConfig authConfig)
        {
            _authConfig = authConfig;
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_authConfig.RefreshTokenSecret)),
                ValidIssuer = _authConfig.Issuer,
                ValidAudience = _authConfig.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(
                    refreshToken,
                    validationParameters,
                    out SecurityToken validatedToken
                );

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
