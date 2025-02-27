namespace ClinicService.BLL.Models.Requests;
public class UpdatePatientRequest : CreatePatientRequest
{
    public required Guid Id { get; set; }
}
