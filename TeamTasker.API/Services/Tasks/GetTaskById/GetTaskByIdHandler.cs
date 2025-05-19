using BuildingBlocks.CQRS.Query;
using TeamTasker.API.Data;

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

            return new GetTaskByIdResult(task);
        }
    }
}
