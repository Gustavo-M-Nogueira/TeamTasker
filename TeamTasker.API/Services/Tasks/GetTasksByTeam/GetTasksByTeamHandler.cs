using BuildingBlocks.CQRS.Query;
using BuildingBlocks.Pagination;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;

namespace TeamTasker.API.Services.Tasks.GetTasksFromTeam
{
    public record GetTasksByTeamQuery(int TeamId, PaginationRequest Request) : IQuery<GetTasksByTeamResult>;
    public record GetTasksByTeamResult(PaginationResult<Models.Entities.Task> Tasks);

    public class GetTasksByTeamQueryValidator : AbstractValidator<GetTasksByTeamQuery>
    {
        public GetTasksByTeamQueryValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class GetTasksByTeamHandler
        (ApplicationDbContext context) 
        : IQueryHandler<GetTasksByTeamQuery, GetTasksByTeamResult>
    {
        public async Task<GetTasksByTeamResult> Handle(GetTasksByTeamQuery query, CancellationToken cancellationToken)
        {
            var teamExists = await context.Teams.AnyAsync(t => t.Id == query.TeamId, cancellationToken);

            if (!teamExists)
                throw new TeamNotFoundException(query.TeamId);

            var tasks = context.Tasks.Where(t => t.TeamId == query.TeamId);

            int pageIndex = query.Request.PageIndex;
            int pageSize = query.Request.PageSize;
            long totalCount = await tasks.LongCountAsync(cancellationToken);
            
            var paginatedTasks = await tasks
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginationResult<Models.Entities.Task>(pageIndex, pageSize, totalCount, paginatedTasks);

            return new GetTasksByTeamResult(result);
        }
    }
}
