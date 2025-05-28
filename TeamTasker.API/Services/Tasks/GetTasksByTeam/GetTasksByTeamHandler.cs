using BuildingBlocks.CQRS.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;

namespace TeamTasker.API.Services.Tasks.GetTasksFromTeam
{
    public record GetTasksByTeamQuery(int TeamId) : IQuery<GetTasksByTeamResult>;
    public record GetTasksByTeamResult(IEnumerable<Models.Task> Tasks);

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

            var tasks = await context.Tasks.Where(t => t.TeamId == query.TeamId).ToListAsync(cancellationToken);

            return new GetTasksByTeamResult(tasks);
        }
    }
}
