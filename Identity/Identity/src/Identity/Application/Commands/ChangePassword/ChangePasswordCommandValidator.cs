namespace Identity.Application.Commands.ChangePassword;

using Common.Validators;
using FluentValidation;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(request => request.CurrentPassword).NotNull().NotEmpty();
        RuleFor(request => request.NewPassword).Password().NotEqual(request => request.CurrentPassword);
    }
}
