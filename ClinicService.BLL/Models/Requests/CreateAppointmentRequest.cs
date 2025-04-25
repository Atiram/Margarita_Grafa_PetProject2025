namespace ClinicService.BLL.Models.Requests;

public class CreateAppointmentRequest
{
    public required Guid DoctorId { get; set; }
    public required Guid PatientId { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
