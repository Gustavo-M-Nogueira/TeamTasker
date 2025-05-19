using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Teams.GetTeamById
{
    //public record GetTeamByIdRequest();
    public record GetTeamByIdResponse(Team Team);
    public class GetTeamByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamByIdQuery(id));

                var response = result.Adapt<GetTeamByIdResponse>();

                return Results.Ok(response);
            })
                .WithName("GetTeamById")
                .Produces<GetTeamByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Team By Id")
                .WithDescription("Get Team By Id");
        }
    }
}
