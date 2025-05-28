using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Tasks.UpdateTaskStatus
{
    public record UpdateTaskStatusRequest(Enums.TaskStatus Status) : ICommand<UpdateTaskStatusResult>;
    public record UpdateTaskStatusResponse(bool IsSuccess);
    public class UpdateTaskStatusEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/teams/{teamId:int}/tasks/{taskId:int}",
                async (int teamId, int taskId, [FromBody] UpdateTaskStatusRequest request, ISender sender) =>
                {
                    var command = new UpdateTaskStatusCommand(teamId, taskId, request.Status);

                    var result = await sender.Send(command);

                    var response = result.Adapt<UpdateTaskStatusResponse>();

                    return Results.Ok(response);
                })
                .WithName("UpdateTaskStatus")
                .Produces<UpdateTaskStatusResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update Task Status")
                .WithDescription("Update Task Status")
                .RequireAuthorization();
        }
    }
}
