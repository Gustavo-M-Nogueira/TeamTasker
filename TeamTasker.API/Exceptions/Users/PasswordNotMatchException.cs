using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class PasswordNotMatchException : BadRequestException
    {
        public PasswordNotMatchException(string message = "Passwords do not match", string? details = null) : base(message, details) { }
    }
}
