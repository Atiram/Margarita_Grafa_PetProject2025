using ClinicService.BLL.Models.Requests;
using ClinicService.BLL.Utilities.Messages;
using FluentValidation;

namespace ClinicService.API.Validators;

public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
{
    public CreateDoctorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage)
            .MinimumLength(2).WithMessage(ClinicNotificationMessages.ValidationMinLengthMessage);

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage)
            .MinimumLength(2).WithMessage(ClinicNotificationMessages.ValidationMinLengthMessage);

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(ClinicNotificationMessages.ValidationInvalidEmailMessage)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage);

        RuleFor(x => x.Specialization)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage);

        RuleFor(x => x.Office)
            .MaximumLength(100).WithMessage(ClinicNotificationMessages.ValidationMaxLengthMessage);

        RuleFor(x => x.CareerStartYear)
            .GreaterThan(1900).WithMessage(ClinicNotificationMessages.ValidationInvalidMaxYearMessage)
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage(ClinicNotificationMessages.ValidationInvalidMaxYearMessage);
    }
}
