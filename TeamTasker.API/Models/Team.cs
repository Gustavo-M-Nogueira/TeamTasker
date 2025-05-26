namespace TeamTasker.API.Models
{
    public class Team
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<User> Users { get; set; } = new List<User>();   
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
