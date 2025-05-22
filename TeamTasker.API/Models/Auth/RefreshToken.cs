namespace TeamTasker.API.Models.Auth
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required Guid UserId { get; set; }
    }
}
