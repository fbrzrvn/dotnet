namespace Identity.Application.Commands.Update;

using Common.Validators;
using FluentValidation;

public class UpdateIdentityUserCommandValidator : AbstractValidator<UpdateIdentityUserCommand>
{
    public UpdateIdentityUserCommandValidator()
    {
        RuleFor(request => request.UserId).Uid();
        RuleFor(request => request.UserName).OptionalUsername();
        RuleFor(request => request.Email).EmailAddress().When(request => string.IsNullOrEmpty(request.Email) == false);
        RuleFor(request => request.PhoneNumber).PhoneNumber();
        RuleFor(request => request.Role).UserRole();
    }
}
