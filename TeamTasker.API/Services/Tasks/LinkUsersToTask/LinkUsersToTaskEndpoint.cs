using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Tasks.LinkUserToTask
{
    public record LinkUsersToTaskRequest(List<Guid> UserIds) : ICommand<LinkUsersToTaskResult>;
    public record LinkUsersToTaskResponse(bool IsSuccess);
    public class LinkUsersToTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/tasks/{id}/users", async ([FromBody] LinkUsersToTaskRequest request, int id, ISender sender) =>
            {
                LinkUsersToTaskCommand command = new LinkUsersToTaskCommand(
                   request.UserIds,
                   id
                );

                var result = await sender.Send(command);

                var response = result.Adapt<LinkUsersToTaskResponse>();

                return response;
            })
                .WithName("LinkUsersToTask")
                .Produces<LinkUsersToTaskResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Link Users To Task")
                .WithDescription("Link Users To Task")
                .RequireAuthorization();
        }
    }
}
