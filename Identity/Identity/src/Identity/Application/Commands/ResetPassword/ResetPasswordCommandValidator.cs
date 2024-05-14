namespace Identity.Application.Commands.ResetPassword;

using Common.Validators;
using FluentValidation;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.Password).Password();
        RuleFor(request => request.Token).Token();
    }
}
