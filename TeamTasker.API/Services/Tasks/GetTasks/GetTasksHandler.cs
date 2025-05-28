using BuildingBlocks.CQRS.Query;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;

namespace TeamTasker.API.Services.Tasks.GetTasks
{
    public record GetTasksQuery(PaginationRequest Request) : IQuery<GetTasksResult>;
    public record GetTasksResult(PaginationResult<Models.Entities.Task> Tasks);

    internal class GetTasksHandler
        (ApplicationDbContext context)
        : IQueryHandler<GetTasksQuery, GetTasksResult>
    {
        public async Task<GetTasksResult> Handle(GetTasksQuery query, CancellationToken cancellationToken)
        {
            var tasks = context.Tasks;

            int pageIndex = query.Request.PageIndex;
            int pageSize = query.Request.PageSize;
            long totalCount = await tasks.LongCountAsync(cancellationToken);

            var paginatedTasks = await tasks
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var result = new PaginationResult<Models.Entities.Task>(pageIndex, pageSize, totalCount, paginatedTasks);

            return new GetTasksResult(result);
        }
    }
}
