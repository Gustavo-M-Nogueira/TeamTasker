using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Enums;

namespace TeamTasker.API.Services.Tasks.CreateTask
{
    public record CreateTaskRequest
        (string Title, string Description, TaskPriority Priority, Enums.TaskStatus Status, int TeamId)
        : ICommand<CreateTaskResult>;
    public record CreateTaskResponse(int Id);
    public class CreateTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/tasks", async (CreateTaskRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateTaskCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateTaskResponse>();

                return Results.Created($"/tasks/{response.Id}", response);
            })
                .WithName("CreateTask")
                .Produces<CreateTaskResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Task")
                .WithDescription("Create Task")
                .RequireAuthorization();
        }
    }
}
