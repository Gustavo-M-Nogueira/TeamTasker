using BuildingBlocks.CQRS.Command;
using TeamTasker.API.Data;
using TeamTasker.API.Enums;

namespace TeamTasker.API.Services.Tasks.CreateTask
{
    public record CreateTaskCommand
        (string Title, string Description, TaskPriority Priority, Enums.TaskStatus Status, int TeamId)
        : ICommand<CreateTaskResult>;
    public record CreateTaskResult(int Id);

    internal class CreateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<CreateTaskCommand, CreateTaskResult>
    {
        public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = new Models.Task
            {
                Title = command.Title,
                Description = command.Description,
                Priority = command.Priority,
                Status = command.Status,
                TeamId = command.TeamId
            };

            context.Tasks.Add(task);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateTaskResult(task.Id);
        }
    }
}
