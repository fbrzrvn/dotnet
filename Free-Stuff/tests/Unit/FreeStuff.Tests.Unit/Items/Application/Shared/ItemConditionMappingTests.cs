using FluentAssertions;
using FluentValidation;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain.Enum;

namespace FreeStuff.Tests.Unit.Items.Application.Shared;

public class ItemConditionMappingTests
{
    [Theory]
    [InlineData("Fair condition", ItemCondition.FairCondition)]
    [InlineData("Good condition", ItemCondition.GoodCondition)]
    [InlineData("As good as new", ItemCondition.AsGoodAsNew)]
    [InlineData("Has given it all", ItemCondition.HasGivenItAll)]
    [InlineData("New", ItemCondition.New)]
    public void MapStringToItemCondition_ShouldMapStringToItemCondition_WhenInputIsValid
    (
        string        conditionString,
        ItemCondition expected
    )
    {
        // Act
        var actual = conditionString.MapExactStringToItemCondition();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MapStringToItemCondition_ShouldThrowValidationException_WhenInputIsNotValid()
    {
        // Arrange
        const string invalidConditionString = "Old but gold";

        // Act
        var actual = () => invalidConditionString.MapExactStringToItemCondition();

        // Assert
        actual.Should().Throw<ValidationException>().WithMessage($"Invalid item condition: {invalidConditionString}");
    }

    [Theory]
    [InlineData(ItemCondition.FairCondition, "Fair condition")]
    [InlineData(ItemCondition.GoodCondition, "Good condition")]
    [InlineData(ItemCondition.AsGoodAsNew, "As good as new")]
    [InlineData(ItemCondition.HasGivenItAll, "Has given it all")]
    [InlineData(ItemCondition.New, "New")]
    public void MapItemConditionToString_ShouldMapItemConditionToString_WhenInputIsValid
    (
        ItemCondition condition,
        string        expected
    )
    {
        // Act
        var actual = condition.MapItemConditionToString();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void MapItemConditionToString_ShouldThrowValidationException_WhenInputIsNotValid()
    {
        // Arrange
        const ItemCondition invalidCondition = (ItemCondition)100;

        // Act
        var actual = () => invalidCondition.MapItemConditionToString();

        // Assert
        actual.Should().Throw<ValidationException>().WithMessage($"Invalid item condition: {invalidCondition}");
    }
}