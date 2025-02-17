namespace ClinicService.API.ViewModels;

public class PatientViewModel : GeneralViewModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public required string PhoneNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
}
