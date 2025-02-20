namespace ClinicService.API.ViewModels;

public class AppointmentViewModel : GeneralViewModel
{
    public Guid DoctorId { get; set; }
    public required DoctorViewModel Doctor { get; set; }
    public Guid PatientId { get; set; }
    public required PatientViewModel Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
