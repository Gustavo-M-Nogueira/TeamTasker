using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Services.Tasks.UpdateTask
{
    public record UpdateTaskCommand
        (TaskRequestDto Task, int TeamId, int TaskId)      
        : ICommand<UpdateTaskResult>; 
    public record UpdateTaskResult(bool IsSuccess);

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Task).NotEmpty().WithMessage("Task can not be empty");
            RuleFor(x => x.Task.Title).NotEmpty().Length(6, 40).WithMessage("Task title must be between 6-40 characters");
            RuleFor(x => x.Task.Description).NotEmpty().Length(6, 200).WithMessage("Task description must be between 6-200 characters");
            RuleFor(x => x.Task.Priority).NotEmpty().IsInEnum().WithMessage("Task priority is required");
            RuleFor(x => x.Task.Status).NotEmpty().IsInEnum().WithMessage("Task status is required");
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required");
        }
    }

    internal class UpdateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.TaskId, cancellationToken);

            if (task is null || task.TeamId != command.TeamId)
                throw new TaskNotFoundException(command.TaskId);
            
            task.Title = command.Task.Title;
            task.Description = command.Task.Description;
            task.Priority = command.Task.Priority;
            task.Status = command.Task.Status;

            context.Update(task);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTaskResult(true);
        }
    }
}
