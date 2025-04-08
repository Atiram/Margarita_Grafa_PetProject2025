namespace ClinicService.BLL.Models.Requests;
public class CreateAppointmentResultRequest
{
    public required string Complaints { get; set; }
    public required string Conclusion { get; set; }
    public required string Recommendations { get; set; }
    public required Guid AppointmentId { get; set; }
}
