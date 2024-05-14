using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Infrastructure;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Constants;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Tests.Unit.Items.Infrastructure;

public class EfItemRepositoryTests : IDisposable
{
    private readonly FreeStuffDbContext _dbContext;
    private readonly IItemRepository    _itemRepository;

    public EfItemRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<FreeStuffDbContext>()
                               .UseInMemoryDatabase($"FreeStuffDBTests_{Guid.NewGuid()}")
                               .Options;

        _dbContext      = new FreeStuffDbContext(dbContextOptions);
        _itemRepository = new EfItemRepository(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddItemToContext_WhenCreated()
    {
        // Arrange
        await CreateItemInContext();

        // Assert
        var actual = _dbContext.Items.Single();
        actual.Id.Should().NotBeNull();
        actual.Title.Should().Be(Constants.Item.Title);
        actual.Description.Should().Be(Constants.Item.Description);
        actual.Category.Name.Should().Be(Constants.Category.Name);
        actual.Condition.Should().Be(Constants.Item.Condition.MapExactStringToItemCondition());
        actual.UserId.Value.Should().Be(Constants.Item.UserId);
        actual.CreatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
        actual.UpdatedDateTime.Should().Be(actual.CreatedDateTime);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnItem_WhenExistsInContext()
    {
        // Arrange
        var expected = await CreateItemInContext();

        // Act
        var actual = await _itemRepository.GetAsync(expected.Id, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyItemsList_WhenTableHasNoRecords()
    {
        // Act
        var actual = await _itemRepository.GetAllAsync(
            Constants.Shared.Page,
            Constants.Shared.Limit,
            CancellationToken.None
        );

        // Assert
        actual.Should().BeEquivalentTo(Enumerable.Empty<Item>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnItems_WhenTableHasRecords()
    {
        // Arrange
        var item = await CreateItemInContext();

        // Act
        var itemsEnumerable = await _itemRepository.GetAllAsync(
            Constants.Shared.Page,
            Constants.Shared.Limit,
            CancellationToken.None
        );

        // Assert
        var actual = itemsEnumerable!.ToArray();
        actual.Should().ContainSingle();
        actual.Should().Contain(item);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyItemsList_WhenTableHasNoRecords()
    {
        // Act
        var actual = await _itemRepository.SearchAsync(
            string.Empty,
            string.Empty,
            Constants.Item.Condition.MapExactStringToItemCondition(),
            string.Empty,
            CancellationToken.None
        );

        // Assert
        actual.Should().BeEquivalentTo(Enumerable.Empty<Item>());
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnItems_WhenTableHasRecords()
    {
        // Arrange
        var item = await CreateItemInContext();

        // Act
        var itemsEnumerable = await _itemRepository.SearchAsync(
            "item",
            string.Empty,
            item.Condition,
            string.Empty,
            CancellationToken.None
        );

        // Assert
        var actual = itemsEnumerable!.ToArray();
        actual.Should().ContainSingle();
        actual.Should().Contain(item);
    }

    [Fact]
    public async Task Update_ShouldUpdateItem_WhenExistsAndInputIsValid()
    {
        // Arrange
        var item     = await CreateItemInContext();
        var category = Category.Create(Constants.Category.EditedName, string.Empty);

        item.Update(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            category,
            Constants.Item.EditedCondition.MapExactStringToItemCondition()
        );

        // Act
        _itemRepository.Update(item);
        await _itemRepository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var actual = _dbContext.Items.Single(i => i.Id == item.Id);
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(item);
        actual.Title.Should().Be(Constants.Item.EditedTitle);
        actual.Description.Should().Be(Constants.Item.EditedDescription);
        actual.Category?.Name.Should().Be(Constants.Category.EditedName);
        actual.Condition.Should().Be(Constants.Item.EditedCondition.MapExactStringToItemCondition());
        actual.UpdatedDateTime.Should().NotBe(actual.CreatedDateTime);
        actual.UpdatedDateTime.Should().BeSameDateAs(DateTime.UtcNow);
    }

    [Fact]
    public async Task Delete_ShouldDeleteItem_WhenExists()
    {
        // Arrange
        var item         = await CreateItemInContext();
        var itemToDelete = await _itemRepository.GetAsync(item.Id, CancellationToken.None);

        // Act
        _itemRepository.Delete(itemToDelete!);
        await _itemRepository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var actual = _dbContext.Items.FirstOrDefault(x => x.Id == itemToDelete!.Id);
        actual.Should().BeNull();
    }

    public void Dispose()
    {
        ClearDatabase();
        _dbContext.Dispose();
    }

    private void ClearDatabase()
    {
        var items = _dbContext.Items.ToList();

        foreach (var item in items)
            _dbContext.Items.Remove(item);

        _dbContext.SaveChanges();
    }

    private async Task<Item> CreateItemInContext()
    {
        var item = ItemUtils.CreateItem();

        await _itemRepository.CreateAsync(item, CancellationToken.None);
        await _itemRepository.SaveChangesAsync(CancellationToken.None);

        return item;
    }
}
