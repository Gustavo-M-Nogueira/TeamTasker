using BuildingBlocks.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;

namespace TeamTasker.API.Services.Tasks.GetTasks
{
    public record GetTasksQuery() : IQuery<GetTasksResult>;
    public record GetTasksResult(IEnumerable<Models.Task> Tasks);

    internal class GetTasksHandler
        (ApplicationDbContext context)
        : IQueryHandler<GetTasksQuery, GetTasksResult>
    {
        public async Task<GetTasksResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await context.Tasks.ToListAsync();

            return new GetTasksResult(tasks);
        }
    }
}
