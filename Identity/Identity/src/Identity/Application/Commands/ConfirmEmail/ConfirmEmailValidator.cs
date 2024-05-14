namespace Identity.Application.Commands.ConfirmEmail;

using Common.Validators;
using FluentValidation;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.Token).Token();
    }
}
