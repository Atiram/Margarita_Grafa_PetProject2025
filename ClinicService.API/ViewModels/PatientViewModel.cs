namespace ClinicService.API.ViewModels;

public class PatientViewModel : GeneralViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
