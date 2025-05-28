namespace TeamTasker.API.Models.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required Guid UserId { get; set; }
    }
}
