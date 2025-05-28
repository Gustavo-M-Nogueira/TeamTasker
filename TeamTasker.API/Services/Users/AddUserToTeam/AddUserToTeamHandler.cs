using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Exceptions.Users;

namespace TeamTasker.API.Services.Users.AddUserToTeam
{
    public record AddUserToTeamCommand(Guid UserId, int TeamId) : ICommand<AddUserToTeamResult>;
    public record AddUserToTeamResult(bool IsSuccess);

    public class AddUserToTeamValidator : AbstractValidator<AddUserToTeamCommand>
    {
        public AddUserToTeamValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }

    internal class AddUserToTeamHandler
        (ApplicationDbContext context)
        : ICommandHandler<AddUserToTeamCommand, AddUserToTeamResult>
    {
        public async Task<AddUserToTeamResult> Handle(AddUserToTeamCommand command, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(command.UserId, cancellationToken);

            if (user is null)
                throw new UserNotFoundException(command.UserId);

            if (user.TeamId == command.TeamId)
                throw new UserAlreadyInATeam("User is already in this team");

            if (user.TeamId != null)
                throw new UserAlreadyInATeam("User is already in another team");

            var teamExist = await context.Teams.AnyAsync(t => t.Id == command.TeamId, cancellationToken);

            if (teamExist == false)
                throw new TeamNotFoundException(command.TeamId);

            user.TeamId = command.TeamId;
            user.Position = Enums.UserPosition.Worker;
            await context.SaveChangesAsync(cancellationToken);

            return new AddUserToTeamResult(true);
        }
    }
}
