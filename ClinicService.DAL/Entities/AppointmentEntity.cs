namespace ClinicService.DAL.Entities;

public class AppointmentEntity : GenericEntity
{
    public required DoctorEntity Doctor { get; set; }
    public required PatientEntity Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }

}
