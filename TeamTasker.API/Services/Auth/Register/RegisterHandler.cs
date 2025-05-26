using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Users;
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

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required in a email format");
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(2).MaximumLength(20).WithMessage("Username must be between 2 - 20 characters");
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20).WithMessage("First name must be between 2 - 20 characters");
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(20).WithMessage("Last name must be between 2 - 20 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Password confirmation is required");
        }
    }

    internal class RegisterHandler
        (ApplicationDbContext context,
        UserManager<User> userManager)
        : ICommandHandler<RegisterCommand, RegisterResult>
    {
        public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (command.Password != command.ConfirmPassword)
                throw new PasswordNotMatchException();

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
