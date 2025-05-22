using BuildingBlocks.CQRS.Command;
using Microsoft.AspNetCore.Identity;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Auth.Register
{
    public record RegisterCommand(
        string Email, 
        string UserName, 
        string FirstName, 
        string LastName,
        string Password,
        string ConfirmPassword) 
        : ICommand<RegisterResult>;
    public record RegisterResult(bool IsSuccess);
    internal class RegisterHandler
        (ApplicationDbContext context,
        UserManager<User> userManager)
        : ICommandHandler<RegisterCommand, RegisterResult>
    {
        public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (command.Password != command.ConfirmPassword)
            {
                return null;
            }

            User registrationUser = new User()
            {
                Email = command.Email,
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
            };

            IdentityResult result = await userManager.CreateAsync(registrationUser, command.Password);

            if (!result.Succeeded)
            {
                IdentityErrorDescriber errorDescriber = new IdentityErrorDescriber();
                IdentityError primaryError = result.Errors.FirstOrDefault();

                if (primaryError.Code == nameof(errorDescriber.DuplicateEmail))
                    return new RegisterResult(false);
                else
                    return new RegisterResult(false);
            }

            return new RegisterResult(true);
        }
    }
}
