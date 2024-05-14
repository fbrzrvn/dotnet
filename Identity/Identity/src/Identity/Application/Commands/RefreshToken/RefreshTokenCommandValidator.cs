namespace Identity.Application.Commands.RefreshToken;

using Common.Validators;
using FluentValidation;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(request => request.RefreshToken).Token();
    }
}
