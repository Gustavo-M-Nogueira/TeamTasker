using BuildingBlocks.CQRS.Query;
using BuildingBlocks.Pagination;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Models.DTOs;

namespace TeamTasker.API.Services.Users.ListUsersFromTeam
{
    public record ListUsersFromTeamQuery(int TeamId, PaginationRequest Request) : IQuery<ListUsersFromTeamResult>;
    public record ListUsersFromTeamResult(PaginationResult<UserDto> Users);

    public class ListUsersFromTeamQueryValidator : AbstractValidator<ListUsersFromTeamQuery>
    {
        public ListUsersFromTeamQueryValidator()
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class ListUsersFromTeamHandler
        (ApplicationDbContext context) 
        : IQueryHandler<ListUsersFromTeamQuery, ListUsersFromTeamResult>
    {
        public async Task<ListUsersFromTeamResult> Handle(ListUsersFromTeamQuery query, CancellationToken cancellationToken)
        {
            var teamExists = await context.Teams.AnyAsync(t => t.Id == query.TeamId, cancellationToken);

            if (!teamExists)
                throw new TeamNotFoundException(query.TeamId);

            var usersInTeam = context.Users.Where(u => u.TeamId == query.TeamId);

            int pageIndex = query.Request.PageIndex;
            int pageSize = query.Request.PageSize;
            long totalCount = await usersInTeam.LongCountAsync(cancellationToken);

            var paginatedUsers = await usersInTeam
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            IEnumerable<UserDto> paginatedUsersDto = paginatedUsers.Select(u => u.Adapt<UserDto>());

            var result = new PaginationResult<UserDto>(pageIndex, pageSize, totalCount, paginatedUsersDto);

            return new ListUsersFromTeamResult(result);
        }
    }
}
