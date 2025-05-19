using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Teams.UpdateTeam
{
    public record UpdateTeamCommand
        (int Id, string Name, string Description, string ImageUrl) 
        : ICommand<UpdateTeamResult>;
    public record UpdateTeamResult(Team Team);
    internal class UpdateTeamHandler
        (ApplicationDbContext context) 
        : ICommandHandler<UpdateTeamCommand, UpdateTeamResult>
    {
        public async Task<UpdateTeamResult> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(command.Id, cancellationToken);

            if (team == null)
            {
                return null;
            }

            team.Name = command.Name;
            team.Description = command.Description;
            team.ImageUrl = command.ImageUrl;

            context.Teams.Update(team);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTeamResult(team);
        }
    }
}
