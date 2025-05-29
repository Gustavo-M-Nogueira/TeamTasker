using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TeamTasker.API.Data;
using TeamTasker.API.Services.Auth.Authorization.Services;

namespace TeamTasker.API.Services.Auth.Authorization.Requirements
{
    public class TeamMemberRequirement : IAuthorizationRequirement
    {
    }

    public class TeamMemberRequirementHandler : AuthorizationHandler<TeamMemberRequirement>
    {
        private readonly IUserAuthorizationService _userAuthorizationService;

        public TeamMemberRequirementHandler(IUserAuthorizationService userAuthorizationService)
        {
            _userAuthorizationService = userAuthorizationService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamMemberRequirement requirement)
        {
            var (user, teamId) = await _userAuthorizationService.GetUserAndTeamIdAsync(context.User);           

            if (user is null || teamId == 0) return;

            if (user.TeamId == teamId)
            {
                context.Succeed(requirement);
            }
        }
    }
}
