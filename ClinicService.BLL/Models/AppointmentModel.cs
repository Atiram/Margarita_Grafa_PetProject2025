using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Models;
public class AppointmentModel : GenericEntity
{
    public required DoctorEntity Doctor { get; set; }
    public required PatientEntity Patient { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly Slots { get; set; }
}
