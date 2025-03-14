namespace NotificationService.DAL.Entities;
public class EventEntity
{
    public Guid? Id { get; set; }
    public int? Type { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
