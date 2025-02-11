using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Models;
public class AppointmentModel : GenericEntity
{
    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Slots { get; set; }
}
