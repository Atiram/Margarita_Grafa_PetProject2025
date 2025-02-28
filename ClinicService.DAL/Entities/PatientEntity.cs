namespace ClinicService.DAL.Entities;

public class PatientEntity : GenericEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public List<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
}

