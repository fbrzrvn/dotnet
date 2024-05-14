namespace Identity.Application.Commands.Create;

using Common.Validators;
using FluentValidation;

public class CreateIdentityUserCommandValidator : AbstractValidator<CreateIdentityUserCommand>
{
    public CreateIdentityUserCommandValidator()
    {
        RuleFor(request => request.UserName).Username();
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.Password).Password();
        RuleFor(request => request.PhoneNumber).PhoneNumber();
        RuleFor(request => request.Role).UserRole();
    }
}
