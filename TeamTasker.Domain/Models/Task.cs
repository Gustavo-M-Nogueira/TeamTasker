using TeamTasker.Domain.Enums;

namespace TeamTasker.Domain.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public List<User>? Users { get; set; }
    }
}
