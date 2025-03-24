namespace ClinicService.BLL.Utilities.Messages;
public static class ClinicNotificationMessages
{
    public const string validationExeptionMessage = "Length must be at least three characters";
    public const string emailSubject = "New appointment is created";
    public const string emailMessageTemplate = "Appointment on {0} at {1}. Patient {2} {3}";
    public const string EventUrlSectionMissingErrorMessage = "Section 'EventUrl' is missing or empty in configuration.";
    public const string HostSectionMissingErrorMessage = "Section 'RabbitMqSettings:Host' is missing or empty in configuration.";
    public const string QueueNameSectionMissingErrorMessage = "Section 'RabbitMqSettings:QueueName' is missing or empty in configuration.";
}