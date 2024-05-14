namespace Identity.Application.Common.Validators;

using FluentValidation;

public static class PasswordValidator
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .Length(8, 24)
            .Matches("(?=.*[A-Z])")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("(?=.*[a-z])")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"(?=.*\d)")
            .WithMessage("Password must contain at least one digit")
            .Matches("(?=.*[!@#$%^&*])")
            .WithMessage("Password must contain at least one special character (!@#$%^&*)");
    }
}
