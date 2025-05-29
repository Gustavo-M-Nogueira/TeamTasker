using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Users.ListUsersFromTeam
{
    public record ListUsersFromTeamResponse(PaginationResult<UserDto> users);
    public class ListUsersFromTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/{teamId:int}/users",
                async ([AsParameters] PaginationRequest request, int teamId, ISender sender) =>
                {
                    var query = new ListUsersFromTeamQuery(teamId, request);

                    var result = await sender.Send(query);

                    var response = result.Adapt<ListUsersFromTeamResponse>();

                    return Results.Ok(response);
                })
                .WithName("ListUsersFromTeam")
                .Produces<ListUsersFromTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("List Users From Team")
                .WithDescription("List Users From Team")
                .RequireAuthorization("TeamMember");
        }
    }
}
