using TeamTasker.API.Models.Entities;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Models.DTOs
{
    public class UserDto
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public Team? Team { get; set; }
        public UserPosition Position { get; set; }
        public List<Entities.Task>? Tasks { get; set; }
    }
}
