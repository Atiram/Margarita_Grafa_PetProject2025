using ClinicService.DAL.Entities;

namespace ClinicService.API.ViewModels;

public class AppointmentViewModel : GeneralViewModel
{
    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Slots { get; set; }
}
