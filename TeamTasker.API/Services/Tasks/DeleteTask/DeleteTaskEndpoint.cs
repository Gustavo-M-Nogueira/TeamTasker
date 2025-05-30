﻿using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Tasks.DeleteTask
{
    public record DeleteTaskResponse(bool IsSuccess);
    public class DeleteTaskEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("teams/{teamId}/tasks/{taskId}", 
                async (int teamId, int taskId, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteTaskCommand(teamId, taskId));

                    var response = result.Adapt<DeleteTaskResponse>();

                    return Results.Ok(response);
                })
                .WithName("DeleteTask")
                .Produces<DeleteTaskResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Task")
                .WithDescription("Delete Task")
                .RequireAuthorization("TeamLeader");
        }
    }
}
