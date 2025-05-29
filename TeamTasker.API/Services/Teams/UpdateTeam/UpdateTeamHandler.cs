using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Teams.UpdateTeam
{
    public record UpdateTeamCommand
        (int TeamId, TeamRequestDto Team) 
        : ICommand<UpdateTeamResult>;
    public record UpdateTeamResult(bool IsSuccess);

    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(c => c.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(c => c.Team).NotEmpty();
            RuleFor(c => c.Team.Name).Length(2, 40).NotEmpty().WithMessage("Name must be beetwen 2 - 40 characters");
        }
    }

    internal class UpdateTeamHandler
        (ApplicationDbContext context) 
        : ICommandHandler<UpdateTeamCommand, UpdateTeamResult>
    {
        public async Task<UpdateTeamResult> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(command.TeamId, cancellationToken);

            if (team is null)
                throw new TeamNotFoundException(command.TeamId);

            team.Name = command.Team.Name;
            team.Description = command.Team.Description;
            team.ImageUrl = command.Team.ImageUrl;

            context.Teams.Update(team);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTeamResult(true);
        }
    }
}
