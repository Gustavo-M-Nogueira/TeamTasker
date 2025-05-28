using BuildingBlocks.CQRS.Query;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Teams.GetTeamById
{
    public record GetTeamByIdQuery(int Id) : IQuery<GetTeamByIdResult>;
    public record GetTeamByIdResult(Team Team);
    internal class GetTeamByIdHandler(ApplicationDbContext context)
        : IQueryHandler<GetTeamByIdQuery, GetTeamByIdResult>
    {
        public async Task<GetTeamByIdResult> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(request.Id, cancellationToken);

            if (team is null)
                throw new TeamNotFoundException(request.Id);

            return new GetTeamByIdResult(team);
        }
    }
}
