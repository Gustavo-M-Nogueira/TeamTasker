using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamTasker.API.Enums;

namespace TeamTasker.API.Services.Users.UpdateUserRoleInTeam
{
    public record UpdateUserPositionRequest(UserPosition Position) 
        : ICommand<UpdateUserPositionResult>;
    public record UpdateUserPositionResponse(bool IsSuccess);

    public class UpdateUserPositionEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/teams/{teamId:int}/users/{userId:Guid}",
                async (int teamId, Guid userId, UpdateUserPositionRequest request, ISender sender) =>
                {
                    var command = new UpdateUserPositionCommand(teamId, userId, request.Position);

                    var result = await sender.Send(command);

                    var response = result.Adapt<UpdateUserPositionResponse>();

                    return Results.Ok(response);
                })
                .WithName("UpdateUserPosition")
                .Produces<UpdateUserPositionResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update User Position")
                .WithDescription("Update User Position")
                .RequireAuthorization();
        }
    }
}
