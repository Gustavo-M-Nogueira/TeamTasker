using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Users.ListUsersFromTeam
{
    public record ListUsersFromTeamResponse(IEnumerable<User> users);
    public class ListUsersFromTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/{teamId:int}/users",
                async (int teamId, ISender sender) =>
                {
                    var query = new ListUsersFromTeamQuery(teamId);

                    var result = await sender.Send(query);

                    var response = result.Adapt<ListUsersFromTeamResponse>();

                    return Results.Ok(response);
                })
                .WithName("ListUsersFromTeam")
                .Produces<ListUsersFromTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("List Users From Team")
                .WithDescription("List Users From Team")
                .RequireAuthorization();
        }
    }
}
