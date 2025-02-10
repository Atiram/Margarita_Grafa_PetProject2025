using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Models;
public class AppointmentModel : GenericEntity
{
    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
    public DateTime Date { get; set; }
    public DateTime Slots { get; set; }
}
