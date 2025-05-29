using BuildingBlocks.CQRS.Query;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;

namespace TeamTasker.API.Services.Tasks.GetTask
{
    public record GetTaskByIdQuery(int TeamId, int TaskId) : IQuery<GetTaskByIdResult>;
    public record GetTaskByIdResult(Models.Entities.Task Task);
    internal class GetTaskByIdHandler(ApplicationDbContext context)
        : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResult>
    {
        public async Task<GetTaskByIdResult> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(query.TaskId, cancellationToken);

            if (task is null || task.TeamId != query.TeamId)
                throw new TaskNotFoundException(query.TaskId);

            return new GetTaskByIdResult(task);
        }
    }
}
