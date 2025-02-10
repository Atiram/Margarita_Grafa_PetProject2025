namespace ClinicService.DAL.Entities;

public class PatientEntity : GenericEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}

