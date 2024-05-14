namespace Identity.Application.Common.Validators;

using System.Text.RegularExpressions;
using FluentValidation;

public static partial class PhoneNumberValidator
{
    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(phoneNumber => string.IsNullOrEmpty(phoneNumber) || PhoneNumberRegex().IsMatch(phoneNumber))
            .WithMessage("Invalid phone number format");
    }

    [GeneratedRegex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")]
    private static partial Regex PhoneNumberRegex();
}
