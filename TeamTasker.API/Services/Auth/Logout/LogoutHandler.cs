using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Users;

namespace TeamTasker.API.Services.Auth.Logout
{
    public record LogoutCommand(Guid UserId) : ICommand<LogoutResult>;
    public record LogoutResult(bool IsSuccess);

    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }

    internal class LogoutHandler 
        (ApplicationDbContext context)
        : ICommandHandler<LogoutCommand, LogoutResult>
    {
        public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == command.UserId, cancellationToken);

            if (!userExists)
                throw new UserNotFoundException(command.UserId);

            var refreshTokens = context.RefreshTokens.Where(t => t.UserId == command.UserId);

            context.RefreshTokens.RemoveRange(refreshTokens);
            
            await context.SaveChangesAsync();

            return new LogoutResult(true);
        }
    }
}
