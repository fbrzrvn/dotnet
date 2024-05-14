using FluentAssertions;
using FreeStuff.Items.Application.GetAll;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Constants;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.GetAll;

public class GetAllItemsQueryHandlerTests
{
    private readonly GetAllItemsQueryHandler _handler;
    private readonly IItemRepository         _itemRepository = Substitute.For<IItemRepository>();
    private readonly IMapper                 _mapper         = Substitute.For<IMapper>();

    public GetAllItemsQueryHandlerTests()
    {
        _handler = new GetAllItemsQueryHandler(_itemRepository, _mapper);
    }

    [Fact]
    public async Task HandleGetAllItemsQueryHandler_ShouldReturnEmptyList_WhenItemsNoExists()
    {
        // Arrange
        var getAllItemQuery = new GetAllItemsQuery(Constants.Shared.Page, Constants.Shared.Limit);

        _itemRepository
            .GetAllAsync(
                getAllItemQuery.Page,
                getAllItemQuery.Limit,
                Arg.Any<CancellationToken>()
            )
            .Returns(new List<Item>());

        _mapper
            .Map<List<ItemDto>>(Arg.Any<List<Item>>())
            .Returns(new List<ItemDto>());

        // Act
        var actual = await _handler.Handle(getAllItemQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Data.Should().BeNullOrEmpty();

        await _itemRepository.Received(1)
                             .GetAllAsync(
                                 Constants.Shared.Page,
                                 Constants.Shared.Limit,
                                 CancellationToken.None
                             );
    }

    [Fact]
    public async Task HandleGetAllItemsQueryHandler_ShouldReturnItemsList_WhenItemsExist()
    {
        // Arrange
        var expectedItems = new List<Item>
        {
            ItemUtils.CreateItem(),
            ItemUtils.CreateItem()
        };

        var expectedItemDtos = expectedItems
                               .Select(_ => ItemUtils.CreateItemDto())
                               .ToList();

        var getAllItemQuery = new GetAllItemsQuery(Constants.Shared.Page, Constants.Shared.Limit);

        _itemRepository
            .GetAllAsync(
                getAllItemQuery.Page,
                getAllItemQuery.Limit,
                Arg.Any<CancellationToken>()
            )
            .Returns(expectedItems);

        _mapper
            .Map<List<ItemDto>>(Arg.Any<List<Item>>())
            .Returns(expectedItemDtos);

        // Act
        var actual = await _handler.Handle(getAllItemQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Data.Should().BeEquivalentTo(expectedItemDtos);

        await _itemRepository.Received(1)
                             .GetAllAsync(
                                 Constants.Shared.Page,
                                 Constants.Shared.Limit,
                                 CancellationToken.None
                             );
    }
}
