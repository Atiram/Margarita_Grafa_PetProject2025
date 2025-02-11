namespace ClinicService.BLL.Models;
public class PatientModel : GenericModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
