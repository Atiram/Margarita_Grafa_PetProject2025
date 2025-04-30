namespace Clinic.Domain;
public static class NotificationMessages
{
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
    public const string NotDeletedErrorMessage = "File is not deleted";
    public const string NoBlobNameErrorMessage = "BlobName is required";
    public const string NoUrlErrorMessage = "Blob URL is invalid or missing.";
    public const string SectionMissingErrorMessage = "Section {0} is missing or empty in configuration.";
    public const string UploadingFileErrorMessage = "Error uploading file. Initiating rollback.";
    public const string RetryingExecuteHttpRequestErrorMessage = "Retrying {0} after {1} with DoctorId: {2}";
    public const string FailedExecuteHttpRequestErrorMessage = "Failed to perform {0} after {1} attempts";
    public const string WritingBlobErrorMessage = "Error writing to MongoDB. Initiating rollback.";
    public const string DeletingBlobErrorMessage = "Error deleting blob during rollback. Manual intervention required!";
    public const string ReminderAppointmentMessageSubject = "Reminder about the upcoming appointment";
    public const string ReminderAppointmentMessageTemplate = "Dear {0} {1}, we remind you about the upcoming appointment {4} at {5} with {3} {2}";
    public const string HangfireJobStartedMessage = "Hangfire job 'SendAppointmentReminders' has started.";
    public const string HangfireJobCompletedMessage = "Hangfire job 'SendAppointmentReminders' has completed.";
    public const string RabbitMQSettMessage = "Message sent to RabbitMQ for doctor {0} regarding appointment {1}";
}
