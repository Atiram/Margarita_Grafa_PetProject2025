using NotificationService.DAL.Enums;

namespace NotificationService.API.ViewModels;

public class EventViewModel
{
    public EventType? Type { get; set; }
    public string? Metadata { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
