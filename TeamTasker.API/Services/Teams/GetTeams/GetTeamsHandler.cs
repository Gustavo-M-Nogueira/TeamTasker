using BuildingBlocks.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Teams.GetTeams
{
    public record GetTeamsQuery() : IQuery<GetTeamsResult>;
    
    public record GetTeamsResult(IEnumerable<Team> Teams);

    internal class GetTeamsQueryHandler(ApplicationDbContext context) : IQueryHandler<GetTeamsQuery, GetTeamsResult>
    {
        public async Task<GetTeamsResult> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await context.Teams.ToListAsync();

            return new GetTeamsResult(teams);
        }
    }
}
