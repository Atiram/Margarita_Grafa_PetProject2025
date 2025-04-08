namespace ClinicService.BLL.Models.Requests;
public class UpdateAppointmentResultRequest : CreateAppointmentResultRequest
{
    public required Guid Id { get; set; }
}
