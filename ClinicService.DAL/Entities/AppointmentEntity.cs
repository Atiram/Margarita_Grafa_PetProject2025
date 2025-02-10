namespace ClinicService.DAL.Entities;

public class AppointmentEntity : GenericEntity
{
    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
    public DateTime Date { get; set; }
    public DateTime Slots { get; set; }

}
