using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Teams.CreateTeam
{
    public record CreateTeamRequest(TeamRequestDto Team)
        : ICommand<CreateTeamResult>;
    public record CreateTeamResponse(int Id);

    public class CreateTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/teams", async (CreateTeamRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateTeamCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateTeamResponse>();

                return Results.Created($"/teams/{response.Id}", response);
            })
                .WithName("CreateTeam")
                .Produces<CreateTeamResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Team")
                .WithDescription("Create Team")
                .RequireAuthorization();
        }
    }
}
