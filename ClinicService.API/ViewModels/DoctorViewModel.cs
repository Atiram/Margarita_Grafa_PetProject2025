using ClinicService.DAL.Enums;

namespace ClinicService.API.ViewModels;

public class DoctorViewModel : GeneralViewModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required string Specialization { get; set; }
    public required string Office { get; set; }
    public required int CareerStartYear { get; set; }
    public required DoctorStatus Status { get; set; }
    public required List<AppointmentViewModel>? Appointments { get; set; }
}
