using TeamTasker.API.Enums;

namespace TeamTasker.API.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public List<User>? Users { get; set; }
    }
}
