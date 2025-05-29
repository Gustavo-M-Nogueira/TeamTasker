using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Users.RemoveUserFromTeam
{
    public record RemoveUserFromTeamResponse(bool IsSuccess);
    public class RemoveUserFromTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/teams/{teamId:int}/users/{userId:Guid}",
                async (int teamId, Guid userId, ISender sender) =>
                {
                    var command = new RemoveUserFromTeamCommand(teamId, userId);

                    var result = await sender.Send(command);

                    var response = result.Adapt<RemoveUserFromTeamResponse>();

                    return Results.Ok(response);
                })
                .WithName("RemoveUserFromTeam")
                .Produces<RemoveUserFromTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Remove User From Team")
                .WithDescription("Remove User From Team")
                .RequireAuthorization("TeamLeader");
        }
    }
}
