using FluentAssertions;
using FreeStuff.Categories.Application.GetAll;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Tests.Utils.Constants;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Categories.Application.GetAll;

public class GetAllCategoryQueryHandlerTests
{
    private readonly GetAllCategoriesQueryHandler _handler;
    private readonly ICategoryRepository          _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMapper                      _mapper             = Substitute.For<IMapper>();

    public GetAllCategoryQueryHandlerTests()
    {
        _handler = new GetAllCategoriesQueryHandler(_categoryRepository, _mapper);
    }

    [Fact]
    public async Task HandleGetAllCategoriesQueryHandler_ShouldReturnEmptyList_WhenCategoriesNoExists()
    {
        // Arrange
        var getAllCategoryQuery = new GetAllCategoriesQuery();

        _categoryRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Category>());

        _mapper.Map<List<CategoryDto>>(Arg.Any<List<Category>>()).Returns(new List<CategoryDto>());

        // Act
        var actual = await _handler.Handle(getAllCategoryQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeNullOrEmpty();

        await _categoryRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleGetAllCategoriesQueryHandler_ShouldReturnCategoriesList_WhenItemsExist()
    {
        // Arrange
        var expectedCategories = new List<Category>
        {
            Category.Create(Constants.Category.Name, Constants.Category.Description)
        };

        var expectedCategoriesDtos = expectedCategories
                                     .Select(
                                         c => new CategoryDto(
                                             c.Id.Value,
                                             c.Name,
                                             c.Description
                                         )
                                     )
                                     .ToList();

        var getAllCategoriesQuery = new GetAllCategoriesQuery();

        _categoryRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(expectedCategories);

        _mapper.Map<List<CategoryDto>>(Arg.Any<List<Category>>()).Returns(expectedCategoriesDtos);

        // Act
        var actual = await _handler.Handle(getAllCategoriesQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expectedCategoriesDtos);

        await _categoryRepository.Received(1).GetAllAsync(CancellationToken.None);
    }
}
