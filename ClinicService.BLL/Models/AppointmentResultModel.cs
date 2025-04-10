using ClinicService.DAL.Entities;

namespace ClinicService.BLL.Models;
public class AppointmentResultModel : GenericModel
{
    public required string Complaints { get; set; }
    public required string Conclusion { get; set; }
    public required string Recommendations { get; set; }
    public required Guid AppointmentId { get; set; }
    public required AppointmentEntity Appointment { get; set; }
}
