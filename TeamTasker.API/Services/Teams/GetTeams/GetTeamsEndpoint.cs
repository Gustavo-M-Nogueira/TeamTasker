using Carter;
using MediatR;
using Mapster;
using TeamTasker.API.Models.Entities;
using BuildingBlocks.Pagination;

namespace TeamTasker.API.Services.Teams.GetTeams
{
    public record GetTeamsResponse(PaginationResult<Team> Teams);
    public class GetTeamsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams", 
                async ([AsParameters] PaginationRequest request, ISender sender) =>
                {
                    var result = await sender.Send(new GetTeamsQuery(request));

                    var response = result.Adapt<GetTeamsResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetTeams")
                .Produces<GetTeamsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Teams")
                .WithDescription("Get Teams")
                .RequireAuthorization();
        }
    }
}
