using BuildingBlocks.CQRS.Query;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;

namespace TeamTasker.API.Services.Tasks.GetTask
{
    public record GetTaskByIdQuery(int Id) : IQuery<GetTaskByIdResult>;
    public record GetTaskByIdResult(Models.Task Task);
    internal class GetTaskByIdHandler(ApplicationDbContext context)
        : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResult>
    {
        public async Task<GetTaskByIdResult> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(request.Id, cancellationToken);

            if (task is null)
                throw new TaskNotFoundException(request.Id);

            return new GetTaskByIdResult(task);
        }
    }
}
