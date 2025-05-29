using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Mapster;
using TeamTasker.API.Data;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Tasks.CreateTask
{
    public record CreateTaskCommand
        (TaskRequestDto Task, int TeamId)
        : ICommand<CreateTaskResult>;
    public record CreateTaskResult(int Id);

    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Task).NotEmpty().WithMessage("Task can not be empty");
            RuleFor(x => x.Task.Title).NotEmpty().MinimumLength(6).MaximumLength(40).WithMessage("Task title is required");
            RuleFor(x => x.Task.Description).NotEmpty().MinimumLength(6).MaximumLength(200).WithMessage("Task description is required");
            RuleFor(x => x.Task.Priority).NotEmpty().IsInEnum().WithMessage("Task priority is required");
            RuleFor(x => x.Task.Status).NotEmpty().IsInEnum().WithMessage("Task status is required");
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class CreateTaskHandler
        (ApplicationDbContext context)
        : ICommandHandler<CreateTaskCommand, CreateTaskResult>
    {
        public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = command.Task.Adapt<Models.Entities.Task>();

            task.TeamId = command.TeamId;            

            context.Tasks.Add(task);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateTaskResult(task.Id);
        }
    }
}
