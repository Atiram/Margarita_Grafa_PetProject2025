namespace ClinicService.BLL.Models.Requests;
public class UpdateAppointmentRequest
{
    public required Guid Id { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid? PatientId { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }

}