﻿namespace ClinicService.API.ViewModels;

public class PatientViewModel : GeneralViewModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required List<AppointmentViewModel> Appointments { get; set; }
}
