using FluentValidation.TestHelper;
using FreeStuff.Categories.Application.Create;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Tests.Unit.Categories.Application.Create;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    [Fact]
    public void CreateCategoryCommandValidator_ShouldNotThrowAValidationError_WhenCommandIsValid()
    {
        // Arrange
        var createCategoryCommand = new CreateCategoryCommand(Constants.Category.Name, Constants.Category.Description);

        // Act
        var actual = _validator.TestValidate(createCategoryCommand);

        // Assert
        actual.ShouldNotHaveValidationErrorFor(request => request.Name);
        actual.ShouldNotHaveValidationErrorFor(request => request.Description);
    }

    [Fact]
    public void CreateCategoryCommandValidator_ShouldThrowAValidationError_WhenCommandIsInvalid()
    {
        // Arrange
        var createCategoryCommand = new CreateCategoryCommand(string.Empty, Constants.Category.InvalidDescriptionLenght);

        // Act
        var actual = _validator.TestValidate(createCategoryCommand);

        // Assert
        actual.ShouldHaveValidationErrorFor(request => request.Name);
        actual.ShouldHaveValidationErrorFor(request => request.Description);
    }
}
