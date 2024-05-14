using FluentValidation;
using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Items.Application.Shared.Mapping;

public static class ItemConditionMapping
{
    public static readonly Dictionary<ItemCondition, string> ConditionMapping = new()
    {
        { ItemCondition.New, "New" },
        { ItemCondition.FairCondition, "Fair condition" },
        { ItemCondition.GoodCondition, "Good condition" },
        { ItemCondition.AsGoodAsNew, "As good as new" },
        { ItemCondition.HasGivenItAll, "Has given it all" },
        { ItemCondition.None, string.Empty }
    };

    public static ItemCondition MapExactStringToItemCondition(this string conditionString)
    {
        foreach (var kvp in ConditionMapping)
            if (kvp.Value.Equals(conditionString, StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }

        throw new ValidationException($"Invalid item condition: {conditionString}");
    }

    public static ItemCondition MapPartialStringToItemCondition(this string conditionString)
    {
        foreach (var kvp in ConditionMapping)
            if (kvp.Value.ToLower().Contains(conditionString.ToLower()))
            {
                return kvp.Key;
            }

        throw new ValidationException($"Invalid item condition: {conditionString}");
    }

    public static string MapItemConditionToString(this ItemCondition condition)
    {
        if (ConditionMapping.TryGetValue(condition, out var conditionString))
        {
            return conditionString;
        }

        throw new ValidationException($"Invalid item condition: {condition}");
    }
}
