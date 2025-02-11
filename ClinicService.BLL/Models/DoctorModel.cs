using ClinicService.DAL.Entities.Enums;

namespace ClinicService.BLL.Models;
public class DoctorModel : GenericModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Specialization { get; set; }
    public string Office { get; set; }
    public int CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
}
