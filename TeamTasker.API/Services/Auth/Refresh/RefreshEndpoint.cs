using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Auth.Refresh
{
    public record RefreshRequest(Guid UserId, string RefreshToken) : ICommand<RefreshResult>;
    public record RefreshResponse(string AccessToken, string RefreshToken);
    public class RefreshEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/refresh", async ([FromBody] RefreshRequest request, ISender sender) =>
            {
                var command = request.Adapt<RefreshCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<RefreshResult>();

                return response;
            })
                .WithName("Refresh")
                .Produces<RefreshResponse>(StatusCodes.Status202Accepted)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Refresh")
                .WithDescription("Refresh");
        }
    }
}
