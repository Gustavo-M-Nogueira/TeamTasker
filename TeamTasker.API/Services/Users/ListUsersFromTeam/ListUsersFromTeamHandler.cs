using BuildingBlocks.CQRS.Query;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;
using TeamTasker.API.Models;

namespace TeamTasker.API.Services.Users.ListUsersFromTeam
{
    public record ListUsersFromTeamQuery(int TeamId) : IQuery<ListUsersFromTeamResult>;
    public record ListUsersFromTeamResult(IEnumerable<User> Users);

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

            var users = await context.Users.Where(u => u.TeamId == query.TeamId).ToListAsync(cancellationToken);

            return new ListUsersFromTeamResult(users);
        }
    }
}
