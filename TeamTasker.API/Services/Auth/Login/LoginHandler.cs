using BuildingBlocks.CQRS.Command;
using Microsoft.AspNetCore.Identity;
using TeamTasker.API.Data;
using TeamTasker.API.Dtos.Response;
using TeamTasker.API.Models;
using TeamTasker.API.Services.Auth.Tokens;

namespace TeamTasker.API.Services.Auth.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
    public record LoginResult(string AccessToken, string RefreshToken);
    internal class LoginHandler
        (UserManager<User> userManager,
        Authenticator authenticator)
        : ICommandHandler<LoginCommand, LoginResult>
    {
        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            User? user = await userManager.FindByEmailAsync(command.Email);

            if (user == null)
                return null;
            
            bool passwordCheck = await userManager.CheckPasswordAsync(user, command.Password);

            if (!passwordCheck)
                return null;

            TokenResponseDto tokenResponse = await authenticator.Authenticate(user);

            return new LoginResult(
                tokenResponse.AccessToken,
                tokenResponse.RefreshToken
            );
        }
    }
}
