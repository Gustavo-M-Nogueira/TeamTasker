using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class EmailOrPasswordIncorrectException : BadRequestException
    {
        public EmailOrPasswordIncorrectException(string message = "Email or password incorrect", string? details = null) : base(message, details) { }
    }
}
