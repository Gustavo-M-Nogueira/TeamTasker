using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Auth
{
    public class NotValidTokenException : BadRequestException
    {
        public NotValidTokenException(string message = "Token not valid", string details = null) : base(message, details) { }
    }
}
