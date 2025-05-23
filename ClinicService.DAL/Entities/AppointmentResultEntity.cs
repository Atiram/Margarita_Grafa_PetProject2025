﻿namespace ClinicService.DAL.Entities;
public class AppointmentResultEntity : GenericEntity
{
    public required string Complaints { get; set; }
    public required string Conclusion { get; set; }
    public required string Recommendations { get; set; }
    public required Guid AppointmentId { get; set; }
    public required AppointmentEntity Appointment { get; set; }
}
