using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Tasks.GetTasks
{
    //public record GetTasksRequest();
    public record GetTasksResponse(IEnumerable<Models.Task> Tasks);
    public class GetTasksEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tasks", async (ISender sender) =>
            {
                var result = await sender.Send(new GetTasksQuery());

                var response = result.Adapt<GetTasksResponse>();

                return Results.Ok(response);
            })
                .WithName("GetTasks")
                .Produces<GetTasksResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Tasks")
                .WithDescription("Get Tasks")
                .RequireAuthorization();
        }
    }
}
