using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid id) : base("User", id) { }
        public UserNotFoundException(string email) : base("User", email) { }
    }
}
