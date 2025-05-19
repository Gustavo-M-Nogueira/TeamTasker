namespace TeamTasker.Domain.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<User>? Users { get; set; }
        public string? ImageUrl { get; set; }
        public List<Task>? Tasks { get; set; }
    }
}
