namespace Clinic.Domain;
public static class NotificationMessages
{
    public const string HostSectionMissingErrorMessage = "Section 'RabbitMqSettings:Host' is missing or empty in configuration.";
    public const string QueueNameSectionMissingErrorMessage = "Section 'RabbitMqSettings:QueueName' is missing or empty in configuration.";
    public const string InvalidAuthErrorMessage = "Invalid username or password.";
    public const string NoArgumentAuthErrorMessage = "Username and Password are required.";
    public const string AuthUserErrorMessage = "Error authenticating user: {0}";
    public const string RegUserErrorMessage = "Error registering user: {0}";
    public const string GettingUserErrorMessage = "Error getting user by Id: {0}";
    public const string GettingAllUserErrorMessage = "Error getting all users.";
    public const string CreatingUserErrorMessage = "Error creating user: {0}";
    public const string UpdatingUserErrorMessage = "Error updating user with Id: {0}";
    public const string DeletingUserErrorMessage = "Error deleting user with Id: {0}";
    public const string DeletingUserSuccessMessage = "User deleted successfully.";
    public const string InternalServerErrorMessage = "Internal Server Error";
    public const string NotFoundErrorMessage = "Item with id {0} not found";
}
