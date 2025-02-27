namespace ClinicService.API.ViewModels;

public class GeneralViewModel
{
    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
