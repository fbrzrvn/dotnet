using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Contracts.Items.Events;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Application.Update;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Domain;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Constants;
using FreeStuff.Tests.Utils.Extensions;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Update;

public class UpdateItemCommandHandlerTests
{
    private readonly UpdateItemCommandHandler _handler;
    private readonly ICategoryRepository      _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IItemRepository          _itemRepository     = Substitute.For<IItemRepository>();
    private readonly IEventBus                _eventBus           = Substitute.For<IEventBus>();
    private readonly IMapper                  _mapper             = Substitute.For<IMapper>();

    public UpdateItemCommandHandlerTests()
    {
        _handler = new UpdateItemCommandHandler(
            _categoryRepository,
            _itemRepository,
            _eventBus,
            _mapper
        );
    }

    [Fact]
    public async Task HandleUpdateItemCommandHandler_ShouldUpdateItem_WhenFound()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand();
        var item              = ItemUtils.CreateItem();
        var category          = Category.Create(Constants.Category.EditedName);

        item.Update(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            category,
            Constants.Item.EditedCondition.MapExactStringToItemCondition()
        );

        var expected = item.MapItemToDto();

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns(item);

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(category);

        _mapper.Map<ItemDto>(Arg.Any<Item>())
               .Returns(expected);

        // Act
        var actual = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expected);

        _itemRepository.Received(1).Update(item);
        await _itemRepository.Received(1).GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>());
        await _categoryRepository.Received(1).GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _itemRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventBus.Received(1).PublishAsync(Arg.Any<ItemUpdated>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleUpdateItemCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand();
        var category          = Category.Create(Constants.Category.EditedName);

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns((Item)null!);

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(category);

        // Act
        var actual = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        actual.ValidateNotFoundError(updateItemCommand.Id);

        await _itemRepository.Received(1).GetAsync(Arg.Any<ItemId>(), CancellationToken.None);
        await _eventBus.DidNotReceive().PublishAsync(Arg.Any<ItemDeleted>(), Arg.Any<CancellationToken>());
    }
}
