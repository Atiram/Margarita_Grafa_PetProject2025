namespace ClinicService.DAL.Entities;

public class AppointmentEntity : GenericEntity
{
    public Guid DoctorId { get; set; }
    public required DoctorEntity Doctor { get; set; }
    public Guid PatientId { get; set; }
    public required PatientEntity Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
