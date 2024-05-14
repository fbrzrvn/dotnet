namespace Identity.Application.Common.Validators;

using Domain.Enum;
using FluentValidation;

public static class RoleValidator
{
    public static IRuleBuilderOptions<T, string?> UserRole<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(role => string.IsNullOrEmpty(role) || BeValidRole(role)).WithMessage("Invalid role");
    }

    private static bool BeValidRole(string? role)
    {
        return role switch
        {
            nameof(Role.Admin)       => true,
            nameof(Role.Member)      => true,
            nameof(Role.RegularUser) => true,
            _                        => false
        };
    }
}
