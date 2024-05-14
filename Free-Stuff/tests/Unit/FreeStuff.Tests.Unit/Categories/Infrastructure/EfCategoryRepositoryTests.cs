using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Categories.Infrastructure;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using FreeStuff.Tests.Utils.Constants;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Tests.Unit.Categories.Infrastructure;

public class EfCategoryRepositoryTests : IDisposable
{
    private readonly FreeStuffDbContext  _dbContext;
    private readonly ICategoryRepository _categoryRepository;

    public EfCategoryRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FreeStuffDbContext>()
                               .UseInMemoryDatabase($"FreeStuffDBTests_{Guid.NewGuid()}")
                               .Options;

        _dbContext          = new FreeStuffDbContext(dbContextOptions);
        _categoryRepository = new EfCategoryRepository(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCategoryToContext_WhenCreated()
    {
        // Act
        await CreateCategoryInContext();

        // Assert
        var actual = _dbContext.Categories.Single();
        actual.Id.Should().NotBeNull();
        actual.Name.Should().Be(Constants.Category.Name);
        actual.Description.Should().Be(Constants.Category.Description);
        actual.CreatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
        actual.UpdatedDateTime.Should().Be(actual.CreatedDateTime);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCategory_WhenExistsInContext()
    {
        // Arrange
        var expected = await CreateCategoryInContext();

        // Act
        var actual = await _categoryRepository.GetAsync(expected.Name, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyCategoryList_WhenTableHasNoRecords()
    {
        // Act
        var actual = await _categoryRepository.GetAllAsync(CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(Enumerable.Empty<Category>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCategories_WhenTableHasRecords()
    {
        // Arrange
        var category = await CreateCategoryInContext();

        // Act
        var categoriesEnumerable = await _categoryRepository.GetAllAsync(CancellationToken.None);

        // Assert
        var actual = categoriesEnumerable!.ToArray();
        actual.Should().ContainSingle();
        actual.Should().Contain(category);
    }

    [Fact]
    public async Task Update_ShouldUpdateCategory_WhenExistsAndInputIsValid()
    {
        // Arrange
        var category = await CreateCategoryInContext();

        category.Update(Constants.Category.EditedName, Constants.Category.EditedDescription);

        // Act
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var actual = _dbContext.Categories.Single(c => c.Name == category.Name);
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(category);
        actual.Name.Should().Be(Constants.Category.EditedName);
        actual.Description.Should().Be(Constants.Category.EditedDescription);
        actual.UpdatedDateTime.Should().NotBe(actual.CreatedDateTime);
        actual.UpdatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
    }

    [Fact]
    public async Task Delete_ShouldDeleteCategory_WhenExists()
    {
        // Arrange
        var category         = await CreateCategoryInContext();
        var categoryToDelete = await _categoryRepository.GetAsync(category.Name, CancellationToken.None);

        // Act
        _categoryRepository.Delete(categoryToDelete!);
        await _categoryRepository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var actual = _dbContext.Categories.FirstOrDefault(c => c.Name == categoryToDelete!.Name);
        actual.Should().BeNull();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private async Task<Category> CreateCategoryInContext()
    {
        var category = Category.Create(Constants.Category.Name, Constants.Category.Description);

        await _categoryRepository.CreateAsync(category, CancellationToken.None);
        await _categoryRepository.SaveChangesAsync(CancellationToken.None);

        return category;
    }
}
