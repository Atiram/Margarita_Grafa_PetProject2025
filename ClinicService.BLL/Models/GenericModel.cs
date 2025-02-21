﻿namespace ClinicService.BLL.Models;
public class GenericModel
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
