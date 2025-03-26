namespace Clinic.Domain;
public static class NotificationMessages
{
    public const string HostSectionMissingErrorMessage = "Section 'RabbitMqSettings:Host' is missing or empty in configuration.";
    public const string QueueNameSectionMissingErrorMessage = "Section 'RabbitMqSettings:QueueName' is missing or empty in configuration.";
    public const string InvalidAuthErrorMessage = "Invalid username or password.";
}
