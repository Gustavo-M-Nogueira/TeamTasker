using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Services.Tasks.UpdateTask
{
    public record UpdateTaskCommand
        (int Id, string Title, string Description, TaskPriority Priority, Models.Enums.TaskStatus Status, int TeamId)      
        : ICommand<UpdateTaskResult>; 
    public record UpdateTaskResult(Models.Entities.Task Task);

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Task ID is required");
            RuleFor(x => x.Title).NotEmpty().Length(6, 40).WithMessage("Task title must be between 6-40 characters");
            RuleFor(x => x.Description).NotEmpty().Length(6, 200).WithMessage("Task description must be between 6-200 characters");
            RuleFor(x => x.Priority).NotEmpty().IsInEnum().WithMessage("Task priority is required");
            RuleFor(x => x.Status).NotEmpty().IsInEnum().WithMessage("Task status is required");
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class UpdateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.Id, cancellationToken);

            if (task is null)
                throw new TaskNotFoundException(command.Id);
            
            task.Title = command.Title;
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
