using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Tests.Unit.Categories.Domain;

public class CategoryTests
{
    [Fact]
    public void Create_ShouldCreateCategory_WhenInputAreValid()
    {
        // Act
        var actual = Category.Create(Constants.Category.Name, Constants.Category.Description);

        // Assert
        actual.Should().NotBeNull();
        actual.Name.Should().Be(Constants.Category.Name);
        actual.Description.Should().Be(Constants.Category.Description);
        actual.CreatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
        actual.UpdatedDateTime.Should().BeSameDateAs(actual.CreatedDateTime);
    }

    [Fact]
    public void Update_ShouldUpdateCategory_WhenInputAreValid()
    {
        // Arrange
        var actual = Category.Create(Constants.Category.Name, Constants.Category.Description);

        // Act
        actual.Update(Constants.Category.EditedName, Constants.Category.EditedDescription);

        // Assert
        actual.Name.Should().Be(Constants.Category.EditedName);
        actual.Description.Should().Be(Constants.Category.EditedDescription);
        actual.UpdatedDateTime.Should().NotBe(actual.CreatedDateTime);
        actual.UpdatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
    }
}
