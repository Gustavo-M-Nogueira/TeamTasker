using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;
using TeamTasker.API.Enums;

namespace TeamTasker.API.Services.Tasks.UpdateTask
{
    public record UpdateTaskCommand
        (int Id, string Name, string Description, TaskPriority Priority, Enums.TaskStatus Status, int TeamId)      
        : ICommand<UpdateTaskResult>; 
    public record UpdateTaskResult(Models.Task Task);
    internal class UpdateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.Id, cancellationToken);

            if (task == null)
            {
                return null;
            }
            
            task.Name = command.Name;
            task.Description = command.Description;
            task.Priority = command.Priority;
            task.Status = command.Status;
            task.TeamId = command.TeamId;

            context.Update(task);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTaskResult(task);
        }
    }
}
