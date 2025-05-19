using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Teams.CreateTeam
{
    public record CreateTeamCommand
        (string Name, string Description, string ImageUrl) 
        : ICommand<CreateTeamResult>;

    public record CreateTeamResult(int Id);

    internal class CreateTeamHandler(ApplicationDbContext context) 
        : ICommandHandler<CreateTeamCommand, CreateTeamResult>
    {
        public async Task<CreateTeamResult> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = new Team
            {
                Name = command.Name,
                Description = command.Description,
                ImageUrl = command.ImageUrl
            };

            context.Add(team);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateTeamResult(team.Id);
        }
    }
}
