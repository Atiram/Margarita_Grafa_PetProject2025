using ClinicService.DAL.Enums;

namespace ClinicService.BLL.Models;
public class DoctorModel : GenericModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public required DateOnly DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required string Specialization { get; set; }
    public required string Office { get; set; }
    public required int CareerStartYear { get; set; }
    public required DoctorStatus Status { get; set; }
    public List<AppointmentModel>? Appointments { get; set; }
}
