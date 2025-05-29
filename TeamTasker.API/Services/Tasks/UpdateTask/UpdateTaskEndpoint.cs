using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Tasks.UpdateTask
{
    public record UpdateTaskRequest(TaskRequestDto Task);
    public record UpdateTaskResponse(bool IsSuccess);
    public class UpdateTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("teams/{teamId:int}/tasks/{taskId:int}", 
                async (int teamId, int taskId, UpdateTaskRequest request, ISender sender) =>
                {
                    var comamnd = new UpdateTaskCommand(
                        request.Task,
                        teamId,
                        taskId);

                    var result = await sender.Send(comamnd);

                    var response = result.Adapt<UpdateTaskResponse>();

                    return Results.Ok(response);
                })
                .WithName("UpdateTask")
                .Produces<UpdateTaskResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update Task")
                .WithDescription("Update Task")
                .RequireAuthorization("TeamLeader");
        }
    }
}
