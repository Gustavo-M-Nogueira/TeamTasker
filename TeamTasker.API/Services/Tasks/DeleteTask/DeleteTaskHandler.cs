using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;

namespace TeamTasker.API.Services.Tasks.DeleteTask
{
    public record DeleteTaskCommand(int Id) : ICommand<DeleteTaskResult>;
    public record DeleteTaskResult(bool IsSuccess);

    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Task ID is required");
        }
    }

    internal class DeleteTaskHandler(ApplicationDbContext context) 
        : ICommandHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.FindAsync(command.Id);

            if (task == null)
                throw new TaskNotFoundException(command.Id);

            context.Remove(task);
            await context.SaveChangesAsync(cancellationToken);
                
            return new DeleteTaskResult(true);
        }
    }
}
