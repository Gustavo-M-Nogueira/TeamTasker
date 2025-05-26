using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Teams.UpdateTeam
{
    public record UpdateTeamRequest(int Id, string Name, string Description, string ImageUrl);
    public record UpdateTeamResponse(Team Team);
    public class UpdateTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("teams", async (UpdateTeamRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateTeamCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateTeamResponse>();

                return new UpdateTeamResponse(response.Team);
            })
                .WithName("UpdateTeam")
                .Produces<UpdateTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update Team")
                .WithDescription("Update Team")
                .RequireAuthorization();
        }
    }
}
