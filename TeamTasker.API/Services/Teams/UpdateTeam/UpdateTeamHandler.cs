using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Teams.UpdateTeam
{
    public record UpdateTeamCommand
        (int Id, string Name, string Description, string ImageUrl) 
        : ICommand<UpdateTeamResult>;
    public record UpdateTeamResult(Team Team);

    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Team ID is required");
            RuleFor(c => c.Name).Length(2, 40).NotEmpty().WithMessage("Name must be beetwen 2 - 40 characters");
        }
    }

    internal class UpdateTeamHandler
        (ApplicationDbContext context) 
        : ICommandHandler<UpdateTeamCommand, UpdateTeamResult>
    {
        public async Task<UpdateTeamResult> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(command.Id, cancellationToken);


            if (team is null)
                throw new TeamNotFoundException(command.Id);

            team.Name = command.Name;
            team.Description = command.Description;
            team.ImageUrl = command.ImageUrl;

            context.Teams.Update(team);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTeamResult(team);
        }
    }
}
