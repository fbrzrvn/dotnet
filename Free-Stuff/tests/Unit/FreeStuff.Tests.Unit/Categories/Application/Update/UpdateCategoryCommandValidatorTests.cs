using FluentValidation.TestHelper;
using FreeStuff.Categories.Application.Update;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Tests.Unit.Categories.Application.Update;

public class UpdateCategoryCommandValidatorTests
{
    private readonly UpdateCategoryCommandValidator _validator = new();

    [Fact]
    public void UpdateCategoryCommandValidator_ShouldNotThrowAValidationError_WhenCommandIsValid()
    {
        // Arrange
        var updateCategoryCommand = new UpdateCategoryCommand(
            Constants.Category.Name,
            Constants.Category.EditedName,
            Constants.Category.Description
        );

        // Act
        var actual = _validator.TestValidate(updateCategoryCommand);

        // Assert
        actual.ShouldNotHaveValidationErrorFor(request => request.Name);
        actual.ShouldNotHaveValidationErrorFor(request => request.NewName);
        actual.ShouldNotHaveValidationErrorFor(request => request.Description);
    }

    [Fact]
    public void UpdateCategoryCommandValidator_ShouldThrowAValidationError_WhenCommandIsInvalid()
    {
        // Arrange
        var updateCategoryCommand = new UpdateCategoryCommand(
            string.Empty,
            string.Empty,
            string.Empty
        );

        // Act
        var actual = _validator.TestValidate(updateCategoryCommand);

        // Assert
        actual.ShouldHaveValidationErrorFor(request => request.Name);
        actual.ShouldHaveValidationErrorFor(request => request.NewName);
        actual.ShouldHaveValidationErrorFor(request => request.Description);
    }
}
