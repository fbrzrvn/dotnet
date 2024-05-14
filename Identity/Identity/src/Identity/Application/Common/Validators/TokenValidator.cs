namespace Identity.Application.Common.Validators;

using FluentValidation;

public static class TokenValidator
{
    public static IRuleBuilderOptions<T, string> Token<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotNull().NotEmpty().Matches("^[A-Za-z0-9+/]+={0,2}$");
    }
}
