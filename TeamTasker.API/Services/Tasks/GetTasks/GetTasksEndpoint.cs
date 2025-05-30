﻿using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Tasks.GetTasks
{
    public record GetTasksResponse(PaginationResult<Models.Entities.Task> Tasks);
    public class GetTasksEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tasks", 
                async ([AsParameters] PaginationRequest request, ISender sender) =>
                {
                    var result = await sender.Send(new GetTasksQuery(request));

                    var response = result.Adapt<GetTasksResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetTasks")
                .Produces<GetTasksResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Tasks")
                .WithDescription("Get Tasks")
                .RequireAuthorization();
        }
    }
}
