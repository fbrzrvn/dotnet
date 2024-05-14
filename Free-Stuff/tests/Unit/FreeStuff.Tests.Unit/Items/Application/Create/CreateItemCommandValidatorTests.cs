using FluentValidation.TestHelper;
using FreeStuff.Items.Application.Create;
using FreeStuff.Tests.Unit.Items.TestUtils;

namespace FreeStuff.Tests.Unit.Items.Application.Create;

public class CreateItemCommandValidatorTests
{
    private readonly CreateItemCommandValidator _validator = new();

    [Fact]
    public void CreateItemCommandValidator_ShouldNotThrowAValidationError_WhenCommandIsValid()
    {
        // Arrange
        var createItemCommand = ItemCommandUtils.NewCreateItemCommand();

        // Act
        var actual = _validator.TestValidate(createItemCommand);

        // Assert
        actual.ShouldNotHaveValidationErrorFor(request => request.Title);
        actual.ShouldNotHaveValidationErrorFor(request => request.Description);
        actual.ShouldNotHaveValidationErrorFor(request => request.CategoryName);
        actual.ShouldNotHaveValidationErrorFor(request => request.Condition);
        actual.ShouldNotHaveValidationErrorFor(request => request.UserId);
    }

    [Fact]
    public void CreateItemCommandValidator_ShouldThrowAValidationError_WhenCommandIsInvalid()
    {
        // Arrange
        var createItemCommand = ItemCommandUtils.NewCreateItemCommand(
            string.Empty,
            default,
            string.Empty,
            "old but gold",
            Guid.Empty
        );

        // Act
        var actual = _validator.TestValidate(createItemCommand);

        // Assert
        actual.ShouldHaveValidationErrorFor(request => request.Title);
        actual.ShouldNotHaveValidationErrorFor(request => request.Description);
        actual.ShouldHaveValidationErrorFor(request => request.CategoryName);
        actual.ShouldHaveValidationErrorFor(request => request.Condition)
              .WithErrorMessage("Invalid item condition: old but gold");
        actual.ShouldHaveValidationErrorFor(request => request.UserId);
    }
}
