﻿using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;

namespace TeamTasker.API.Services.Tasks.GetTasksFromTeam
{
    public record GetTasksByTeamResponse(PaginationResult<Models.Entities.Task> tasks);
    public class GetTasksByTeamEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/{teamId:int}/tasks",
                async ([AsParameters] PaginationRequest request, int teamId, ISender sender) =>
                {
                    var query = new GetTasksByTeamQuery(teamId, request);

                    var result = await sender.Send(query);

                    var response = result.Adapt<GetTasksByTeamResponse>();

                    return Results.Ok(response);
                })
                .WithName("GetTasksByTeam")
                .Produces<GetTasksByTeamResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Tasks By Team")
                .WithDescription("Get Tasks By Team")
                .RequireAuthorization("TeamMember");
        }
    }
}
