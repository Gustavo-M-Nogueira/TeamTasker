using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Teams.CreateTeam
{
    public record CreateTeamRequest(string Name, string Description, string? ImageUrl)
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

                return new CreateTeamResponse(response.Id);
            })
                .WithName("CreateTeam")
                .Produces<CreateTeamResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Team")
                .WithDescription("Create Team");
        }
    }
}
