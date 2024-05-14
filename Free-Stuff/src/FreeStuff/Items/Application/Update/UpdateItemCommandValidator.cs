using FluentValidation;
using FreeStuff.Items.Application.Shared.Mapping;

namespace FreeStuff.Items.Application.Update;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(request => request.Id).NotEmpty();
        RuleFor(request => request.Title).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Description).NotEmpty().MaximumLength(500);
        RuleFor(request => request.CategoryName).NotEmpty();
        RuleFor(request => request.Condition)
            .Must(BeValidEnumValue)
            .WithMessage(request => $"Invalid item condition: {request.Condition}");
        RuleFor(request => request.UserId).NotEmpty();
    }

    private static bool BeValidEnumValue(string condition)
    {
        foreach (var kvp in ItemConditionMapping.ConditionMapping)
        {
            if (kvp.Value.Equals(condition, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
