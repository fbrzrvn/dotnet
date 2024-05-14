using FluentAssertions;
using FreeStuff.Categories.Application.Delete;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Tests.Utils.Constants;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Categories.Application.Delete;

public class DeleteCategoryCommandHandlerTests
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly ICategoryRepository          _categoryRepository = Substitute.For<ICategoryRepository>();

    public DeleteCategoryCommandHandlerTests()
    {
        _handler = new DeleteCategoryCommandHandler(_categoryRepository);
    }

    [Fact]
    public async Task HandleDeleteCategoryCommandHandler_ShouldDeleteCategoryAndReturnTrue_WhenFound()
    {
        // Arrange
        var category              = Category.Create(Constants.Category.Name, Constants.Category.Description);
        var deleteCategoryCommand = new DeleteCategoryCommand(category.Name);

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(category);

        // Act
        var actual = await _handler.Handle(deleteCategoryCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeTrue();

        _categoryRepository.Received(1).Delete(category);
        await _categoryRepository.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandlerDeleteCategoryCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var deleteCategoryCommand = new DeleteCategoryCommand(Constants.Category.Name);

        // Act
        var actual = await _handler.Handle(deleteCategoryCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Code.Should().Be("Category.NotFoundError");
        actual.FirstError.Description.Should().Be($"Category with name: {deleteCategoryCommand.Name} does not exist");

        _categoryRepository.DidNotReceive().Delete(Arg.Any<Category>());
        await _categoryRepository.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }
}
