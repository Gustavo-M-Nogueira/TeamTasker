using TeamTasker.API.Data;
using TeamTasker.API.Dtos.Response;
using TeamTasker.API.Models;
using TeamTasker.API.Models.Auth;
using TeamTasker.API.Services.Auth.Tokens.TokenGenerators;

namespace TeamTasker.API.Services.Auth.Tokens
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly ApplicationDbContext _context;

        public Authenticator(
            AccessTokenGenerator accessTokenGenerator, 
            RefreshTokenGenerator refreshTokenGenerator, 
            ApplicationDbContext context)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _context = context;
        }

        public async Task<TokenResponseDto> Authenticate(User user)
        {
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDto = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id,
            };

            _context.RefreshTokens.Add(refreshTokenDto);
            await _context.SaveChangesAsync();

            return new TokenResponseDto {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
