using FluentAssertions;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Application.Update;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Tests.Utils.Constants;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Categories.Application.Update;

public class UpdateCategoryCommandHandlerTests
{
    private readonly UpdateCategoryCommandHandler _handler;
    private readonly ICategoryRepository          _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMapper                      _mapper             = Substitute.For<IMapper>();

    public UpdateCategoryCommandHandlerTests()
    {
        _handler = new UpdateCategoryCommandHandler(_categoryRepository, _mapper);
    }

    [Fact]
    public async Task HandleUpdateCategoryCommandHandler_ShouldUpdateCategory_WhenFound()
    {
        // Arrange
        var updateCategoryCommand = new UpdateCategoryCommand(
            Constants.Category.Name,
            Constants.Category.EditedName,
            Constants.Category.Description
        );
        var category = Category.Create(Constants.Category.Name, Constants.Category.Description);

        category.Update(Constants.Category.EditedName, Constants.Category.Description);

        var expected = new CategoryDto(
            category.Id.Value,
            category.Name,
            category.Description
        );

        _categoryRepository
            .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(category);

        _mapper.Map<CategoryDto>(Arg.Any<Category>())
               .Returns(expected);

        // Act
        var actual = await _handler.Handle(updateCategoryCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expected);

        await _categoryRepository.Received(1).GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        _categoryRepository.Received(1).Update(category);
        await _categoryRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleUpdateCategoryCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var updateCategoryCommand = new UpdateCategoryCommand(
            Constants.Category.Name,
            Constants.Category.EditedName,
            Constants.Category.Description
        );

        _categoryRepository
            .GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Category)null!);

        // Act
        var actual = await _handler.Handle(updateCategoryCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Code.Should().Be("Category.NotFoundError");
        actual.FirstError.Description.Should().Be($"Category with name: {updateCategoryCommand.Name} does not exist");

        await _categoryRepository.Received(1).GetAsync(Arg.Any<string>(), CancellationToken.None);
        await _categoryRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
