namespace ClinicService.BLL.Utilities.Messages;
public static class ClinicNotificationMessages
{
    public const string EmailSubject = "New appointment is created";
    public const string EmailMessageTemplate = "Appointment on {0} at {1}. Patient {2} {3}";
    public const string ValidationRequiredFieldMessage = "Field is required";
    public const string ValidationMinLengthMessage = "Length must be at least three characters";
    public const string ValidationMaxLengthMessage = "Length must not exceed 100 characters";
    public const string ValidationInvalidEmailMessage = "Invalid email format";
    public const string ValidationInvalidMinYearMessage = "Invalid career start year";
    public const string ValidationInvalidMaxYearMessage = "Career start year cannot be in the future";
    public const string ValidationInvalidPhoneMessage = "Invalid phone number format";
    public const string ValidationInvalidPhoneLengthMessage = "Phone number must not exceed 20 characters";
    public const string ValidationInvalidDateOfBirthMessage = "Invalid date of birth";
}