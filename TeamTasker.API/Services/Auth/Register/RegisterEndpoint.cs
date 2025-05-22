using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasker.API.Services.Auth.Register
{
    public class RegisterEndpoint : ICarterModule
    {
        public record RegisterRequest(
            string Email, 
            string UserName, 
            string FirstName, 
            string LastName,
            string Password,
            string ConfirmPassword) : ICommand<RegisterResult>;
        public record RegisterResponse(bool IsSuccess);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async ([FromBody] RegisterRequest request, ISender sender) =>
            {
                var command = request.Adapt<RegisterCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<RegisterResponse>();

                return Results.Accepted();
            })
                .WithName("Register")
                .Produces<RegisterResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Register")
                .WithDescription("Register");
        }
    }
}
