namespace ClinicService.BLL.Models.Requests;
public class UpdateDoctorRequest : CreateDoctorRequest
{
    public required Guid Id { get; set; }
}
