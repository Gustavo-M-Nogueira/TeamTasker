using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;

namespace TeamTasker.API.Services.Tasks.DeleteTask
{
    public record DeleteTaskCommand(int TeamId, int TaskId) : ICommand<DeleteTaskResult>;
    public record DeleteTaskResult(bool IsSuccess);

    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required");
        }
    }

    internal class DeleteTaskHandler(ApplicationDbContext context) 
        : ICommandHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.TaskId);

            if (task == null || task.TeamId != command.TeamId)
                throw new TaskNotFoundException(command.TaskId);

            context.Remove(task);
            await context.SaveChangesAsync(cancellationToken);
                
            return new DeleteTaskResult(true);
        }
    }
}
