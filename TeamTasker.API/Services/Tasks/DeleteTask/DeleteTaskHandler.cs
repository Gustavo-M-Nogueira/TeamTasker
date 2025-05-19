using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;

namespace TeamTasker.API.Services.Tasks.DeleteTask
{
    public record DeleteTaskCommand(int Id) : ICommand<DeleteTaskResult>;
    public record DeleteTaskResult(bool IsSuccess);
    internal class DeleteTaskHandler(ApplicationDbContext context) 
        : ICommandHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.Id);
           
            if (task == null)
                return new DeleteTaskResult(false);

            context.Remove(task);
            await context.SaveChangesAsync(cancellationToken);
                
            return new DeleteTaskResult(true);
        }
    }
}
