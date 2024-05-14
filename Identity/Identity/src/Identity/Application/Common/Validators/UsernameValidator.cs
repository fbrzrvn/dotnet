namespace Identity.Application.Common.Validators;

using System.Text.RegularExpressions;
using FluentValidation;

public static partial class UsernameValidator
{
    public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .Length(3, 20)
            .Matches("^[a-zA-Z']")
            .WithMessage("Username must start with a letter")
            .Matches("[a-zA-Z0-9_-]")
            .WithMessage("Username can include only letters, numbers, underscores, and hyphens");
    }

    public static IRuleBuilderOptions<T, string?> OptionalUsername<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(username => string.IsNullOrEmpty(username) || UsernameRegex().IsMatch(username))
            .WithMessage(
                "Username must start with a letter and can include only letters, numbers, underscores and hyphens"
            );
    }

    [GeneratedRegex(@"^[a-zA-Z][a-zA-Z0-9_-]{2,19}$")]
    private static partial Regex UsernameRegex();
}
