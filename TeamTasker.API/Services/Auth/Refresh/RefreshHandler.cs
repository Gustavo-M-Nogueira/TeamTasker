using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Users;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;
using TeamTasker.API.Services.Auth.Tokens;
using TeamTasker.API.Services.Auth.Tokens.TokenValidators;

namespace TeamTasker.API.Services.Auth.Refresh
{
    public record RefreshCommand(Guid UserId, string RefreshToken) : ICommand<RefreshResult>;
    public record RefreshResult(string AccessToken, string RefreshToken);

    public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
        }
    }

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
                throw new NotFiniteNumberException();

            RefreshToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == command.RefreshToken, cancellationToken);

            if (refreshToken is null)
                throw new NotFiniteNumberException();

            User? user = await context.Users.FindAsync(refreshToken.UserId, cancellationToken);

            if (user is null || user.Id != command.UserId)
                throw new UserNotFoundException(command.UserId);

            context.Remove(refreshToken);
            await context.SaveChangesAsync(cancellationToken);

            TokenResponseDto response = await authenticator.Authenticate(user);

            return new RefreshResult(response.AccessToken, response.RefreshToken);
        }
    }
}
