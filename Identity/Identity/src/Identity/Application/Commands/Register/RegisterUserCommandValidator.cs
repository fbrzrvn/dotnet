namespace Identity.Application.Commands.Register;

using Common.Validators;
using FluentValidation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(request => request.UserName).Username();
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.Password).Password();
        RuleFor(request => request.PhoneNumber).PhoneNumber();
    }
}
