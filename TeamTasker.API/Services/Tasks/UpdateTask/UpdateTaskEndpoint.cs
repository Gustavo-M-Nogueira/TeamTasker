using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Services.Tasks.UpdateTask
{
    public record UpdateTaskRequest
        (int Id, string Name, string Description, TaskPriority Priority, Models.Enums.TaskStatus Status, int TeamId);
    public record UpdateTaskResponse(Models.Entities.Task Task);
    public class UpdateTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/tasks", async (UpdateTaskRequest request, ISender sender) =>
            {
                var comamnd = request.Adapt<UpdateTaskCommand>();

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
                .RequireAuthorization();
        }
    }
}
