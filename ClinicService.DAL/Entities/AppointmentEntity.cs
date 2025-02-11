namespace ClinicService.DAL.Entities;

public class AppointmentEntity : GenericEntity
{
    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Slots { get; set; }

}
