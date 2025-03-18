﻿namespace NotificationService.BLL.Models;
public class CreateEventMail
{
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = "Your appointment is created";
    public string Message { get; set; } = "Your appointment is created"!;
    public DateTime OrderDate { get; set; }
}
