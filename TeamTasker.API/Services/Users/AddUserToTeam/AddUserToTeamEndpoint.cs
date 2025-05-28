using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Users.AddUserToTeam
{
    public record AddUserToTeamResponse(bool IsSuccess);
    public class AddUserToTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teams/{teamId:int}/users/{userId:guid}", 
                async (int teamId, Guid userId, ISender sender) =>
            {
                var command = new AddUserToTeamCommand(userId, teamId);

                var result = await sender.Send(command);

                var response = result.Adapt<AddUserToTeamResponse>();

                return Results.Ok(response);
            })
                .WithName("AddUserToTeam")
                .Produces<AddUserToTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Add User To Team")
                .WithDescription("Add User To Team")
                .RequireAuthorization();
        }
    }
}
