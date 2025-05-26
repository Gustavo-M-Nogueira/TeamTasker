using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Tasks.GetTask
{
    //public record GetTaskByIdRequest()
    public record GetTaskByIdResponse(Models.Task Task);
    public class GetTaskByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tasks/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetTaskByIdQuery(id));

                var response = result.Adapt<GetTaskByIdResponse>();

                return Results.Ok(response);
            })
                .WithName("GetTaskById")
                .Produces<GetTaskByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Task By Id")
                .WithDescription("Get Task By Id")
                .RequireAuthorization();
        }
    }
}
