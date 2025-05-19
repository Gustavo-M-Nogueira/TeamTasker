namespace TeamTasker.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public List<Task>? Tasks { get; set; }
    }
}
