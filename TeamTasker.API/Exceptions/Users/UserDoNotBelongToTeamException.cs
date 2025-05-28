using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class UserDoNotBelongToTeamException : BadRequestException
    {
        public UserDoNotBelongToTeamException(string message, string? details = null) : base(message, details) { }
    }
}
