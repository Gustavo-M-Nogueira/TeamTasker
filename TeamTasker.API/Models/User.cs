using Microsoft.AspNetCore.Identity;

namespace TeamTasker.API.Models
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public List<Task>? Tasks { get; set; }
    }
}
