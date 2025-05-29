using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Tasks.GetTask
{
    public record GetTaskByIdResponse(Models.Entities.Task Task);
    public class GetTaskByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("teams/{teamId}/tasks/{taskId}", 
                async (int teamId, int taskId, ISender sender) =>
                {
                    var query = new GetTaskByIdQuery(teamId, taskId);

                    var result = await sender.Send(query);

                    var response = result.Adapt<GetTaskByIdResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetTaskById")
                .Produces<GetTaskByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Task By Id")
                .WithDescription("Get Task By Id")
                .RequireAuthorization("TeamMember");
        }
    }
}
