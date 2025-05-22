using TeamTasker.API.Dtos;

namespace TeamTasker.API.Services.Auth.Tokens.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly AuthConfig _authConfig;

        public RefreshTokenGenerator(TokenGenerator tokenGenerator, AuthConfig authConfig)
        {
            _tokenGenerator = tokenGenerator;
            _authConfig = authConfig;
        }

        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(
                _authConfig.RefreshTokenSecret,
                _authConfig.Issuer,
                _authConfig.Audience,
                _authConfig.RefreshTokenExpirationMinutes
            );
        }
    }
}
