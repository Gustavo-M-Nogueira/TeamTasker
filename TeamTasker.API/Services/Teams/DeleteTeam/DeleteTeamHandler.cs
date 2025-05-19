using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;

namespace TeamTasker.API.Services.Teams.DeleteTeam
{
    public record DeleteTeamCommand(int Id) : ICommand<DeleteTeamResult>;
    public record DeleteTeamResult(bool IsSuccess);
    internal class DeleteTeamHandler
        (ApplicationDbContext context)
        : ICommandHandler<DeleteTeamCommand, DeleteTeamResult>
    {
        public async Task<DeleteTeamResult> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(command.Id, cancellationToken);

            if (team == null)
            {
                return new DeleteTeamResult(false);
            }

            context.Teams.Remove(team);
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteTeamResult(true);
        }
    }
}
