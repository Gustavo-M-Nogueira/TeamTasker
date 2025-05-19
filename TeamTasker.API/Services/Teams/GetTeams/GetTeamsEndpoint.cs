using Carter;
using MediatR;
using TeamTasker.API.Models;
using Mapster;

namespace TeamTasker.API.Services.Teams.GetTeams
{
    //public record GetTeamsRequest();
    public record GetTeamsResponse(IEnumerable<Team> Teams);
    public class GetTeamsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams", async (ISender sender) =>
            {
                var result = await sender.Send(new GetTeamsQuery());

                var response = result.Adapt<GetTeamsResponse>();

                return Results.Ok(response);
            })
                .WithName("GetTeams")
                .Produces<GetTeamsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Teams")
                .WithDescription("Get Teams");
        }
    }
}
