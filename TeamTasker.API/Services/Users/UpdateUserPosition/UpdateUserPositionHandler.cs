using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Exceptions.Users;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Services.Users.UpdateUserRoleInTeam
{
    public record UpdateUserPositionCommand(int TeamId, Guid UserId, UserPosition Position) 
        : ICommand<UpdateUserPositionResult>;
    public record UpdateUserPositionResult(bool IsSuccess);

    public class UpdateUserPositionCommandValidator : AbstractValidator<UpdateUserPositionCommand>
    {
        public UpdateUserPositionCommandValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required");
            RuleFor(x => x.Position).IsInEnum().NotEmpty().WithMessage("User position is required");
        }
    }

    internal class UpdateUserPositionHandler
        (ApplicationDbContext context)
        : ICommandHandler<UpdateUserPositionCommand, UpdateUserPositionResult>
    {
        public async Task<UpdateUserPositionResult> Handle(UpdateUserPositionCommand command, CancellationToken cancellationToken)
        {
            if (command.Position == UserPosition.NotInATeam)
                throw new InvalidOperationException();

            var teamExist = await context.Teams.AnyAsync(t => t.Id == command.TeamId, cancellationToken);
            
            if (!teamExist)
                throw new TeamNotFoundException(command.TeamId);

            var user = await context.Users.FindAsync(command.UserId, cancellationToken);

            if (user is null)
                throw new UserNotFoundException(command.UserId);

            if (user.TeamId != command.TeamId) 
                throw new UserDoNotBelongToTeamException("User do not belong to this team");

            user.Position = command.Position;
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateUserPositionResult(true);
        }
    }
}
