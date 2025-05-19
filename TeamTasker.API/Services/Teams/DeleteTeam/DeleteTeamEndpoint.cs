using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Teams.DeleteTeam
{
    //public record DeleteTeamRequest();
    public record DeleteTeamResponse(bool IsSuccess);
    public class DeleteTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/teams/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteTeamCommand(id));

                var response = result.Adapt<DeleteTeamResponse>();

                return Results.Ok(response);
            })
                .WithName("DeleteTeam")
                .Produces<DeleteTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Team")
                .WithDescription("Delete Team");
        }
    }
}
