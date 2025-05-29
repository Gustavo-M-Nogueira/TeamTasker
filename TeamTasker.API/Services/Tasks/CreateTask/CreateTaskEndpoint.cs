using BuildingBlocks.CQRS.Command;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Tasks.CreateTask
{
    public record CreateTaskRequest(TaskRequestDto Task) : ICommand<CreateTaskResult>;
    public record CreateTaskResponse(int Id);
    public class CreateTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("teams/{teamId:int}/tasks", 
                async (int teamId, [FromBody] CreateTaskRequest request, ISender sender) =>
                {
                    var command = new CreateTaskCommand(
                        request.Task,
                        teamId);

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateTaskResponse>();

                    return Results.Created($"/tasks/{response.Id}", response);
                })
                .WithName("CreateTask")
                .Produces<CreateTaskResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Task")
                .WithDescription("Create Task")
                .RequireAuthorization("TeamLeader");
        }
    }
}
