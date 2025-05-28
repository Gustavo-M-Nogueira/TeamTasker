using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public UserPosition Position { get; set; }
        public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
        
        [NotMapped]
        public List<Task> Tasks => UserTasks.Select(ut => ut.Task).ToList();
    }
}
