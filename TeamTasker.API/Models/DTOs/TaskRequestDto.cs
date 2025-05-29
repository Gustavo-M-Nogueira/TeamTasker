using TeamTasker.API.Models.Enums;

namespace TeamTasker.API.Models.DTOs
{
    public record TaskRequestDto(
        string Title = default!, 
        string Description = default!, 
        TaskPriority Priority = TaskPriority.Low, 
        Enums.TaskStatus Status = Enums.TaskStatus.Pending);
}
