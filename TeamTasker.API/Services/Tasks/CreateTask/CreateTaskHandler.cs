using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Models.Enums;
using TeamTasker.API.Services.Tasks.UpdateTask;

namespace TeamTasker.API.Services.Tasks.CreateTask
{
    public record CreateTaskCommand
        (string Title, string Description, TaskPriority Priority, Models.Enums.TaskStatus Status, int TeamId)
        : ICommand<CreateTaskResult>;
    public record CreateTaskResult(int Id);

    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(6).MaximumLength(40).WithMessage("Task ID is required");
            RuleFor(x => x.Description).NotEmpty().MinimumLength(6).MaximumLength(200).WithMessage("Task ID is required");
            RuleFor(x => x.Priority).NotEmpty().IsInEnum().WithMessage("Task priority is required");
            RuleFor(x => x.Status).NotEmpty().IsInEnum().WithMessage("Task status is required");
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class CreateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<CreateTaskCommand, CreateTaskResult>
    {
        public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = new Models.Entities.Task
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
