using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Exceptions.Users;

namespace TeamTasker.API.Services.Users.RemoveUserFromTeam
{
    public record RemoveUserFromTeamCommand(int TeamId, Guid UserId) : ICommand<RemoveUserFromTeamResult>;
    public record RemoveUserFromTeamResult(bool IsSuccess);

    public class RemoveUserFromTeamCommandValidator : AbstractValidator<RemoveUserFromTeamCommand>
    {
        public RemoveUserFromTeamCommandValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }

    internal class RemoveUserFromTeamHandler
        (ApplicationDbContext context)
        : ICommandHandler<RemoveUserFromTeamCommand, RemoveUserFromTeamResult>
    {
        public async Task<RemoveUserFromTeamResult> Handle(RemoveUserFromTeamCommand command, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(command.UserId, cancellationToken);

            if (user is null)
                throw new UserNotFoundException(command.UserId);

            if (user.TeamId == null || user.TeamId != command.TeamId)
                throw new UserDoNotBelongToTeamException("User do not belong to this team");

            var teamExist = await context.Teams.AnyAsync(t => t.Id == command.TeamId, cancellationToken);

            if (teamExist == false)
                throw new TeamNotFoundException(command.TeamId);

            user.TeamId = null;
            await context.SaveChangesAsync(cancellationToken);

            return new RemoveUserFromTeamResult(true);
        }
    }
}
