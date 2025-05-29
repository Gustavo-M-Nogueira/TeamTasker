using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeamTasker.API.Data;
using TeamTasker.API.Models.Enums;
using TeamTasker.API.Services.Auth.Authorization.Services;

namespace TeamTasker.API.Services.Auth.Authorization.Requirements
{

    public class TeamLeaderRequirement : IAuthorizationRequirement
    {
    }

    public class TeamLeaderRequirementHandler : AuthorizationHandler<TeamLeaderRequirement>
    {
        private readonly IUserAuthorizationService _userAuthorizationService;

        public TeamLeaderRequirementHandler(IUserAuthorizationService userAuthorizationService)
        {
            _userAuthorizationService = userAuthorizationService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamLeaderRequirement requirement)
        {
            var (user, teamId) = await _userAuthorizationService.GetUserAndTeamIdAsync(context.User);

            if (user is null || teamId == 0) return;

            if (user.TeamId == teamId && user.Position == UserPosition.Leader)
            {
                context.Succeed(requirement);
            }
        }
    }
}
