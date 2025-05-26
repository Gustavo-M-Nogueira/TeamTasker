namespace TeamTasker.API.Models
{
    public class UserTask
    {
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public required int TaskId { get; set; }
        public Task Task { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
