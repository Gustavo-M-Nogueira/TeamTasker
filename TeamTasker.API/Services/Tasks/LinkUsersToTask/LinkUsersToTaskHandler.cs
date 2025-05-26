using BuildingBlocks.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Tasks.LinkUserToTask
{
    public record LinkUsersToTaskCommand(List<Guid> UserIds, int TaskId) : ICommand<LinkUsersToTaskResult>;
    public record LinkUsersToTaskResult(bool IsSuccess);
    internal class LinkUsersToTaskHandler
        (ApplicationDbContext context) 
        : ICommandHandler<LinkUsersToTaskCommand, LinkUsersToTaskResult>
    {
        public async Task<LinkUsersToTaskResult> Handle(LinkUsersToTaskCommand command, CancellationToken cancellationToken)
        {
            var taskExists = await context.Tasks.AnyAsync(t => t.Id == command.TaskId, cancellationToken);

            if (!taskExists)
                //"Task not found"
                return new LinkUsersToTaskResult(false);

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
                return new LinkUsersToTaskResult(false);

            context.UserTasks.AddRange(userTasks);
            await context.SaveChangesAsync(cancellationToken);

            return new LinkUsersToTaskResult(true);
        }
    }
}
