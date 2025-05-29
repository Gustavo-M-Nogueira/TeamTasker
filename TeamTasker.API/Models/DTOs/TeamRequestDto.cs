namespace TeamTasker.API.Models.DTOs
{
    public record TeamRequestDto(
        string Name,
        string? Description,
        string? ImageUrl
        );
}
