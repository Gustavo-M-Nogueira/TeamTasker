using System.Security.Claims;
using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Auth.Logout
{
    public record LogoutCommand(Guid UserId, string AccessToken) : ICommand<LogoutResult>;
    public record LogoutResult(bool IsSuccess);
    internal class LogoutHandler 
        (ApplicationDbContext context)
        : ICommandHandler<LogoutCommand, LogoutResult>
    {
        public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            var refreshTokens = context.RefreshTokens.Where(t => t.UserId == command.UserId);
            
            context.RemoveRange(refreshTokens);
            
            await context.SaveChangesAsync();

            return new LogoutResult(true);
        }
    }
}
