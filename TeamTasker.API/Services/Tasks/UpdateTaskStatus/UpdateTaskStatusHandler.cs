using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;

namespace TeamTasker.API.Services.Tasks.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(int TeamId, int TaskId, Models.Enums.TaskStatus Status) : ICommand<UpdateTaskStatusResult>;
    public record UpdateTaskStatusResult(bool IsSuccess);

    public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required");
            RuleFor(x => x.Status).IsInEnum().NotEmpty().WithMessage("Task status is required");
        }
    }

    internal class UpdateTaskStatusHandler
        (ApplicationDbContext context) 
        : ICommandHandler<UpdateTaskStatusCommand, UpdateTaskStatusResult>
    {
        public async Task<UpdateTaskStatusResult> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.TaskId, cancellationToken);

            if (task is null || task.TeamId != command.TeamId)
                throw new TaskNotFoundException(command.TaskId);

            task.Status = command.Status;
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateTaskStatusResult(true);
        }
    }
}
