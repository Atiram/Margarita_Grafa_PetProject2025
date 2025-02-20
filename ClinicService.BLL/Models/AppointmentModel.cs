namespace ClinicService.BLL.Models;

public class AppointmentModel : GenericModel
{
    public Guid DoctorId { get; set; }
    public required DoctorModel Doctor { get; set; }
    public Guid PatientId { get; set; }
    public required PatientModel Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
