using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Tasks.LinkUserToTask
{
    public record AssignUsersToTaskRequest(List<Guid> UserIds) : ICommand<AssignUsersToTaskResult>;
    public record AssignUsersToTaskResponse(bool IsSuccess);
    public class AssignUsersToTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("teams/{teamId}/tasks/{taskId}/users", 
                async (int teamId, int taskId, [FromBody] AssignUsersToTaskRequest request, ISender sender) =>
                {
                    AssignUsersToTaskCommand command = new AssignUsersToTaskCommand(
                       request.UserIds,
                       teamId,
                       taskId
                    );

                    var result = await sender.Send(command);

                    var response = result.Adapt<AssignUsersToTaskResponse>();

                    return response;
                })
                .WithName("AssignUsersToTask")
                .Produces<AssignUsersToTaskResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Assign Users To Task")
                .WithDescription("Assign Users To Task")
                .RequireAuthorization("TeamLeader");
        }
    }
}
