using FluentAssertions;
using FreeStuff.Items.Application.Get;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Extensions;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Get;

public class GetItemQueryHandlerTests
{
    private readonly GetItemQueryHandler _handler;
    private readonly IItemRepository     _itemRepository = Substitute.For<IItemRepository>();
    private readonly IMapper             _mapper         = Substitute.For<IMapper>();

    public GetItemQueryHandlerTests()
    {
        _handler = new GetItemQueryHandler(_itemRepository, _mapper);
    }

    [Fact]
    public async Task HandleGetItemQueryHandler_ShouldReturnItem_WhenFound()
    {
        // Arrange
        var getItemQuery = new GetItemQuery(Guid.NewGuid());
        var item         = ItemUtils.CreateItem();
        var expected     = item.MapItemToDto();

        _itemRepository.GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())!
                       .ReturnsForAnyArgs(Task.FromResult(item));

        _mapper.Map<ItemDto>(Arg.Any<Item>())
               .Returns(expected);

        // Act
        var actual = await _handler.Handle(getItemQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expected);

        await _itemRepository.Received(1).GetAsync(Arg.Any<ItemId>(), CancellationToken.None);
    }

    [Fact]
    public async Task HandleGetItemQueryHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var item         = ItemUtils.CreateItem();
        var getItemQuery = new GetItemQuery(Guid.Parse(item.Id.Value.ToString()));

        _itemRepository.GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
                       .Returns((Item)null!);

        _mapper.Map<ItemDto>(Arg.Any<Item>())
               .Returns(ItemUtils.CreateItemDto());

        // Act
        var actual = await _handler.Handle(getItemQuery, CancellationToken.None);

        // Assert
        actual.ValidateNotFoundError(item.Id.Value);

        await _itemRepository.Received(1).GetAsync(item.Id, CancellationToken.None);
    }
}
