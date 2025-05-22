using BuildingBlocks.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Dtos.Response;
using TeamTasker.API.Models;
using TeamTasker.API.Models.Auth;
using TeamTasker.API.Services.Auth.Tokens;
using TeamTasker.API.Services.Auth.Tokens.TokenValidators;

namespace TeamTasker.API.Services.Auth.Refresh
{
    public record RefreshCommand(Guid UserId, string RefreshToken) : ICommand<RefreshResult>;
    public record RefreshResult(string AccessToken, string RefreshToken);
    internal class RefreshHandler
        (ApplicationDbContext context,
        RefreshTokenValidator refreshTokenValidator,
        Authenticator authenticator)
        : ICommandHandler<RefreshCommand, RefreshResult>
    {
        public async Task<RefreshResult> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            bool IsValidRefreshToken = refreshTokenValidator.Validate(command.RefreshToken);

            if (!IsValidRefreshToken)
            {
                return null;
            }

            RefreshToken refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == command.RefreshToken);

            if (refreshToken == null)
                return null;

            User user = await context.Users.FindAsync(refreshToken.UserId, cancellationToken);

            if (user == null || user.Id != command.UserId)
                return null;

            context.Remove(refreshToken);
            await context.SaveChangesAsync();

            TokenResponseDto response = await authenticator.Authenticate(user);

            return new RefreshResult(response.AccessToken, response.RefreshToken);
        }
    }
}
