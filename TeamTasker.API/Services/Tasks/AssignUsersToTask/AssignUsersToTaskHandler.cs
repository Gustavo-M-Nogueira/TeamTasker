using BuildingBlocks.CQRS.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Tasks;
using TeamTasker.API.Exceptions.Users;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Tasks.LinkUserToTask
{
    public record AssignUsersToTaskCommand(List<Guid> UserIds, int TaskId) : ICommand<AssignUsersToTaskResult>;
    public record AssignUsersToTaskResult(bool IsSuccess);

    public class AssignUsersToTaskCommandValidator : AbstractValidator<AssignUsersToTaskCommand>
    {
        public AssignUsersToTaskCommandValidator()
        {
            RuleFor(x => x.UserIds).NotEmpty().WithMessage("User ID(s) is/are required");
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required");
        }
    }

    internal class AssignUsersToTaskHandler
        (ApplicationDbContext context) 
        : ICommandHandler<AssignUsersToTaskCommand, AssignUsersToTaskResult>
    {
        public async Task<AssignUsersToTaskResult> Handle(AssignUsersToTaskCommand command, CancellationToken cancellationToken)
        {
            var taskExists = await context.Tasks.AnyAsync(t => t.Id == command.TaskId, cancellationToken);

            if (!taskExists)
                throw new TaskNotFoundException(command.TaskId);

            List<UserTask> userTasks = new List<UserTask>();

            var existingUserIds = await context.Users
                .Where(u => command.UserIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);

            var existingLinks = await context.UserTasks
                .Where(ut => ut.TaskId == command.TaskId && command.UserIds.Contains(ut.UserId))
                .Select(ut => ut.UserId)
                .ToListAsync(cancellationToken);

            foreach (var userId in command.UserIds)
            {
                if (!existingUserIds.Contains(userId))
                    continue;
                if (existingLinks.Contains(userId))
                    continue;

                userTasks.Add(new UserTask
                {
                    UserId = userId,
                    TaskId = command.TaskId
                });
            }

            if (!userTasks.Any())
                throw new UsersNotFoundOrAlreadyLinkedException();

            context.UserTasks.AddRange(userTasks);
            await context.SaveChangesAsync(cancellationToken);

            return new AssignUsersToTaskResult(true);
        }
    }
}
