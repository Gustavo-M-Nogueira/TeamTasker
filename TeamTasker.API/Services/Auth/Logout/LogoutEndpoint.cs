using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Auth.Logout
{
    public record LogoutRequest(Guid UserId,  string AccessToken);
    public record LogoutResponse(bool IsSuccess);
    public class LogoutEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/logout", async ([FromBody] LogoutRequest request, ISender sender) =>
            {
                var command = request.Adapt<LogoutCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<LogoutResponse>();

                return response;
            })
                .WithName("Logout")
                .Produces<LogoutResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Logout")
                .WithDescription("Logout");
        }
    }
}
