﻿namespace ClinicService.BLL.Models.Requests;
public class UpdateAppointmentRequest : CreateAppointmentRequest
{
    public required Guid Id { get; set; }
}