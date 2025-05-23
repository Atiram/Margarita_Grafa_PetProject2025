﻿using ClinicService.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace ClinicService.BLL.Models.Requests;
public class CreateDoctorRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; } = string.Empty;
    public required DateOnly DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required string Specialization { get; set; }
    public required string Office { get; set; }
    public required int CareerStartYear { get; set; }
    public required DoctorStatus Status { get; set; }
    public IFormFile? Formfile { get; set; }
}
