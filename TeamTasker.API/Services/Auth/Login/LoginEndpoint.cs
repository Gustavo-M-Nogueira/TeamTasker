using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Auth.Login
{
    public record LoginRequest(string Email, string Password) : ICommand<LoginResult>;
    public record LoginResponse(string AccessToken, string RefreshToken);
    public class LoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async ([FromBody] LoginRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<LoginResponse>();

                return Results.Accepted("/login", response);
            })
                .WithName("Login")
                .Produces<LoginResponse>(StatusCodes.Status202Accepted)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Login")
                .WithDescription("Login");
        }
    }
}
