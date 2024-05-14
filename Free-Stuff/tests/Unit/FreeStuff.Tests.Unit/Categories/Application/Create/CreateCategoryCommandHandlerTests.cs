using FluentAssertions;
using FreeStuff.Categories.Application.Create;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Tests.Utils.Constants;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Categories.Application.Create;

public class CreateCategoryCommandHandlerTests
{
    private readonly CreateCategoryCommandHandler _handler;
    private readonly ICategoryRepository          _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMapper                      _mapper             = Substitute.For<IMapper>();

    public CreateCategoryCommandHandlerTests()
    {
        _handler = new CreateCategoryCommandHandler(_categoryRepository, _mapper);
    }

    [Fact]
    public async Task HandleCreateCategoryCommand_ShouldCreateAndReturnCategory_WhenCategoryIsValid()
    {
        // Arrange
        var createCategoryCommand = new CreateCategoryCommand(Constants.Category.Name, Constants.Category.Description);

        _categoryRepository.CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>())
                           .ReturnsForAnyArgs(
                               Task.FromResult(Category.Create(Constants.Category.Name, Constants.Category.Description))
                           );

        _mapper.Map<CategoryDto>(Arg.Any<Category>())
               .Returns(
                   new CategoryDto(
                       Guid.NewGuid(),
                       Constants.Category.Name,
                       Constants.Category.Description
                   )
               );

        // Act
        var actual = await _handler.Handle(createCategoryCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Id.Should().NotBeEmpty();
        actual.Value.Name.Should().Be(createCategoryCommand.Name);

        await _categoryRepository.Received(1).CreateAsync(Arg.Any<Category>(), CancellationToken.None);
        await _categoryRepository.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
