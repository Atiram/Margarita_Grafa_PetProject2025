namespace ClinicService.DAL.Entities;

public class GenericEntity
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}