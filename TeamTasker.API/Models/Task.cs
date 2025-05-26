using TeamTasker.API.Enums;

namespace TeamTasker.API.Models
{
    public class Task
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required TaskPriority Priority { get; set; }
        public required Enums.TaskStatus Status { get; set; }
        public required int TeamId { get; set; }
        public Team? Team { get; set; }
        public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
