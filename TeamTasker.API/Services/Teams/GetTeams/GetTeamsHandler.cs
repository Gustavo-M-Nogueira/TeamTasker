using BuildingBlocks.CQRS.Query;
using BuildingBlocks.Pagination;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Teams.GetTeams
{
    public record GetTeamsQuery(PaginationRequest Request) : IQuery<GetTeamsResult>;
    
    public record GetTeamsResult(PaginationResult<Team> Teams);

    internal class GetTeamsQueryHandler
        (ApplicationDbContext context) 
        : IQueryHandler<GetTeamsQuery, GetTeamsResult>
    {
        public async Task<GetTeamsResult> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
        {
            int pageIndex = query.Request.PageIndex;
            int pageSize = query.Request.PageSize;

            var teams = context.Teams;

            long totalCount = await teams.LongCountAsync(cancellationToken);

            var paginatedTeams = await teams
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginationResult<Team>(pageIndex, pageSize, totalCount, paginatedTeams);

            return new GetTeamsResult(result);
        }
    }
}
