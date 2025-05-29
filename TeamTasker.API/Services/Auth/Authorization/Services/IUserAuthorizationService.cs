using System.Security.Claims;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Auth.Authorization.Services
{
    public interface IUserAuthorizationService
    {
        Task<(User? user, int teamId)> GetUserAndTeamIdAsync(ClaimsPrincipal user);
    }
}
