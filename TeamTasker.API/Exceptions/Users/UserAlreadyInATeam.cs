using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class UserAlreadyInATeam : BadRequestException
    {
        public UserAlreadyInATeam(string message, string? details = null) : base(message, details) { }
    }
}
