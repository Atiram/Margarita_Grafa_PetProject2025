namespace ClinicService.DAL.Entities;

public class AppointmentEntity : GenericEntity
{
    public Guid? DoctorId { get; set; }
    public DoctorEntity? Doctor { get; set; }
    public Guid? PatientId { get; set; }
    public PatientEntity? Patient { get; set; }
    public AppointmentResultEntity? AppointmentResult { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
