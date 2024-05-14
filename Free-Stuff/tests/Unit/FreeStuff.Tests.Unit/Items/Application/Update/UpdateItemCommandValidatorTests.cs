using FluentValidation.TestHelper;
using FreeStuff.Items.Application.Update;
using FreeStuff.Tests.Unit.Items.TestUtils;

namespace FreeStuff.Tests.Unit.Items.Application.Update;

public class UpdateItemCommandValidatorTests
{
    private readonly UpdateItemCommandValidator _validator = new();

    [Fact]
    public void UpdateItemCommandValidator_ShouldNotThrowAValidationError_WhenCommandIsValid()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand();

        // Act
        var result = _validator.TestValidate(updateItemCommand);

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.Id);
        result.ShouldNotHaveValidationErrorFor(request => request.Title);
        result.ShouldNotHaveValidationErrorFor(request => request.Description);
        result.ShouldNotHaveValidationErrorFor(request => request.CategoryName);
        result.ShouldNotHaveValidationErrorFor(request => request.Condition);
        result.ShouldNotHaveValidationErrorFor(request => request.UserId);
    }

    [Fact]
    public void UpdateItemCommandValidator_ShouldThrowAValidationError_WhenCommandIsInvalid()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "old but gold",
            Guid.Empty
        );

        // Act
        var actual = _validator.TestValidate(updateItemCommand);

        // Assert
        actual.ShouldHaveValidationErrorFor(request => request.Id);
        actual.ShouldHaveValidationErrorFor(request => request.Title);
        actual.ShouldHaveValidationErrorFor(request => request.Description);
        actual.ShouldHaveValidationErrorFor(request => request.CategoryName);
        actual.ShouldHaveValidationErrorFor(request => request.Condition)
              .WithErrorMessage("Invalid item condition: old but gold");
        actual.ShouldHaveValidationErrorFor(request => request.UserId);
    }
}
