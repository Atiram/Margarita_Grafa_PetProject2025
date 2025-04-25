using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Utilities.Messages;
using FluentValidation;

namespace ClinicService.API.Validators;

public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ClinicNotificationMessages.ValidationRequiredFieldMessage)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage)
            .MinimumLength(2).WithMessage(ClinicNotificationMessages.ValidationMinLengthMessage);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ClinicNotificationMessages.ValidationRequiredFieldMessage)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage)
            .MinimumLength(2).WithMessage(ClinicNotificationMessages.ValidationMinLengthMessage);

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage(ClinicNotificationMessages.ValidationInvalidPhoneLengthMessage);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Now.AddYears(-1))).WithMessage(ClinicNotificationMessages.ValidationInvalidDateOfBirthMessage);
    }
}