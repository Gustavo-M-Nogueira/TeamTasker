using BuildingBlocks.CQRS.Query;
using TeamTasker.API.Data;

namespace TeamTasker.API.Services.Teams.DeleteTeam
{
    public record DeleteTeamQuery(int Id) : IQuery<DeleteTeamResult>;
    public record DeleteTeamResult(bool IsSuccess);
    internal class DeleteTeamHandler
        (ApplicationDbContext context)
        : IQueryHandler<DeleteTeamQuery, DeleteTeamResult>
    {
        public async Task<DeleteTeamResult> Handle(DeleteTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(request.Id, cancellationToken);

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
