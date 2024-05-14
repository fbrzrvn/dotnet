using FluentAssertions;
using FreeStuff.Contracts.Items.Events;
using FreeStuff.Items.Application.Delete;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Domain;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Extensions;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Delete;

public class DeleteItemCommandHandlerTests
{
    private readonly DeleteItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<IItemRepository>();
    private readonly IEventBus                _eventBus       = Substitute.For<IEventBus>();

    public DeleteItemCommandHandlerTests()
    {
        _handler = new DeleteItemCommandHandler(_itemRepository, _eventBus);
    }

    [Fact]
    public async Task HandlerDeleteItemCommandHandler_ShouldDeleteItemAndReturnTrue_WhenFound()
    {
        // Arrange
        var item              = ItemUtils.CreateItem();
        var deleteItemCommand = new DeleteItemCommand(Guid.Parse(item.Id.Value.ToString()));

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns(item);

        // Act
        var actual = await _handler.Handle(deleteItemCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeTrue();

        _itemRepository.Received(1).Delete(item);
        await _itemRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        await _eventBus.Received(1).PublishAsync(Arg.Any<ItemDeleted>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandlerDeleteItemCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var item              = ItemUtils.CreateItem();
        var deleteItemCommand = new DeleteItemCommand(Guid.Parse(item.Id.Value.ToString()));

        // Act
        var actual = await _handler.Handle(deleteItemCommand, CancellationToken.None);

        // Assert
        actual.ValidateNotFoundError(item.Id.Value);

        _itemRepository.DidNotReceive().Delete(item);
        await _itemRepository.DidNotReceive().SaveChangesAsync(CancellationToken.None);
        await _eventBus.DidNotReceive().PublishAsync(Arg.Any<ItemDeleted>(), Arg.Any<CancellationToken>());
    }
}
