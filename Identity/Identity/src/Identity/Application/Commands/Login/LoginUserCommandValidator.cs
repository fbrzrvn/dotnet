namespace Identity.Application.Commands.Login;

using FluentValidation;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(request => request.UserName).NotNull().NotEmpty();
        RuleFor(request => request.Password).NotNull().NotEmpty();
    }
}
