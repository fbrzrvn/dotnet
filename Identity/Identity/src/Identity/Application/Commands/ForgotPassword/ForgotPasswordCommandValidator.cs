namespace Identity.Application.Commands.ForgotPassword;

using FluentValidation;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(request => request.Email).EmailAddress();
    }
}
