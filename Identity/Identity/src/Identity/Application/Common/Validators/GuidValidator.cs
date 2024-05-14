namespace Identity.Application.Common.Validators;

using FluentValidation;

public static class GuidValidator
{
    public static IRuleBuilderOptions<T, string> Uid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().Must(IsValidGuid).WithMessage("Invalid GUID format. Please provide a valid GUID");
    }

    private static bool IsValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
