using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Teams.UpdateTeam
{
    public record UpdateTeamRequest(TeamRequestDto Team);
    public record UpdateTeamResponse(bool IsSuccess);
    public class UpdateTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("teams/{teamId:int}", 
                async (int teamId, UpdateTeamRequest request, ISender sender) =>
                {
                    var command = new UpdateTeamCommand(teamId, request.Team);

                    var result = await sender.Send(command);

                    var response = result.Adapt<UpdateTeamResponse>();

                    return Results.Ok(response);
                })
                .WithName("UpdateTeam")
                .Produces<UpdateTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Update Team")
                .WithDescription("Update Team")
                .RequireAuthorization("TeamLeader");
        }
    }
}
