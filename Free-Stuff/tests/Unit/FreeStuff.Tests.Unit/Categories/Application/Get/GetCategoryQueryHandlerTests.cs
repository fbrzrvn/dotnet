using FluentAssertions;
using FreeStuff.Categories.Application.Get;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Tests.Utils.Constants;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Categories.Application.Get;

public class GetCategoryQueryHandlerTests
{
    private readonly GetCategoryQueryHandler _handler;
    private readonly ICategoryRepository     _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMapper                 _mapper             = Substitute.For<IMapper>();

    public GetCategoryQueryHandlerTests()
    {
        _handler = new GetCategoryQueryHandler(_categoryRepository, _mapper);
    }

    [Fact]
    public async Task HandleGetCategoryQueryHandler_ShouldReturnCategory_WhenFound()
    {
        // Arrange
        var category = Category.Create(Constants.Category.Name);
        var expected = new CategoryDto(
            category.Id.Value,
            category.Name,
            category.Description
        );
        var getCategoryQuery = new GetCategoryQuery(Constants.Category.Name);

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())!
                           .ReturnsForAnyArgs(Task.FromResult(category));

        _mapper.Map<CategoryDto>(Arg.Any<Category>())
               .Returns(expected);

        // Act
        var actual = await _handler.Handle(getCategoryQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expected);

        await _categoryRepository.Received(1).GetAsync(Arg.Any<string>(), CancellationToken.None);
    }

    [Fact]
    public async Task HandleGetItemQueryHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var category = Category.Create(Constants.Category.Name);
        var categoryDto = new CategoryDto(
            category.Id.Value,
            category.Name,
            category.Description
        );
        var getCategoryQuery = new GetCategoryQuery(Constants.Category.Name);

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                           .Returns((Category)null!);

        _mapper.Map<CategoryDto>(Arg.Any<Category>())
               .Returns(categoryDto);

        // Act
        var actual = await _handler.Handle(getCategoryQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Code.Should().Be("Category.NotFoundError");
        actual.FirstError.Description.Should().Be($"Category with name: {getCategoryQuery.Name} does not exist");

        await _categoryRepository.Received(1).GetAsync(Arg.Any<string>(), CancellationToken.None);
    }
}
