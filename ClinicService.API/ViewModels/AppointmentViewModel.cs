using ClinicService.DAL.Entities;

namespace ClinicService.API.ViewModels;

public class AppointmentViewModel : GeneralViewModel
{
    public required DoctorEntity Doctor { get; set; }
    public required PatientEntity Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
