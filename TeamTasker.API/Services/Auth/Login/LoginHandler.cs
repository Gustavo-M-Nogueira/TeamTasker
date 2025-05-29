using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TeamTasker.API.Exceptions.Users;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;
using TeamTasker.API.Services.Auth.Tokens;

namespace TeamTasker.API.Services.Auth.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
    public record LoginResult(string AccessToken, string RefreshToken);

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required in a email format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }

    internal class LoginHandler
        (UserManager<User> userManager,
        Authenticator authenticator)
        : ICommandHandler<LoginCommand, LoginResult>
    {
        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            User? user = await userManager.FindByEmailAsync(command.Email);

            if (user is null)
                throw new EmailOrPasswordIncorrectException();
            
            bool passwordCheck = await userManager.CheckPasswordAsync(user, command.Password);

            if (!passwordCheck)
                throw new EmailOrPasswordIncorrectException();

            TokenResponseDto tokenResponse = await authenticator.Authenticate(user);

            return new LoginResult(
                tokenResponse.AccessToken,
                tokenResponse.RefreshToken
            );
        }
    }
}
