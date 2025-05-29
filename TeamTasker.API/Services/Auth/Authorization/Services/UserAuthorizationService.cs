using System.Security.Claims;
using TeamTasker.API.Data;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Auth.Authorization.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(User? user, int teamId)> GetUserAndTeamIdAsync(ClaimsPrincipal userContext)
        {
            try
            {
                Guid userId = Guid.Empty;
                Guid.TryParse(userContext.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);

                var user = await _dbContext.Users.FindAsync(userId);

                var httpContext = _httpContextAccessor.HttpContext;

                var routeValues = httpContext.Request.RouteValues;

                routeValues.TryGetValue("teamId", out var teamIdObj);
                int.TryParse(teamIdObj?.ToString(), out var teamId);

                return (user, teamId);
            }
            catch (Exception ex)
            {
                return (null, 0);
            }
            //Guid userId = Guid.Empty;
            //Guid.TryParse(userContext.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
            //if (userId == Guid.Empty) return (null, 0);

            //var user = await _dbContext.Users.FindAsync(userId);
            //if (user is null) return (null, 0);


            //var httpContext = _httpContextAccessor.HttpContext;
            //if (httpContext is null) return (null, 0);

            //var routeValues = httpContext.Request.RouteValues;

            //if (!routeValues.TryGetValue("teamId", out var teamIdObj)) return (null, 0);
            //if (!int.TryParse(teamIdObj?.ToString(), out var teamId)) return (null, 0);

            //return (user, teamId);
        }
    }
}
