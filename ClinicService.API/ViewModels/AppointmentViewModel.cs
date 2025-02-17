using ClinicService.DAL.Entities;

namespace ClinicService.API.ViewModels;

public class AppointmentViewModel : GeneralViewModel
{
    public required DoctorEntity Doctor { get; set; }
    public required PatientEntity Patient { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Slots { get; set; }
}
