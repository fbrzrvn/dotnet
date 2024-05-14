using FluentAssertions;
using FreeStuff.Items.Application.Search;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Tests.Unit.Items.TestUtils;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Search;

public class SearchItemsQueryHandlerTests
{
    private readonly SearchItemsQueryHandler _handler;
    private readonly IItemRepository         _itemRepository = Substitute.For<IItemRepository>();
    private readonly IMapper                 _mapper         = Substitute.For<IMapper>();

    public SearchItemsQueryHandlerTests()
    {
        _handler = new SearchItemsQueryHandler(_itemRepository, _mapper);
    }

    [Fact]
    public async Task HandleSearchItemsQueryHandler_ShouldReturnsEmptyList_WhenItemsNoExists()
    {
        // Arrange
        var searchItemsQuery = new SearchItemsQuery(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );

        _itemRepository.SearchAsync(
                           Arg.Any<string>(),
                           Arg.Any<string>(),
                           Arg.Any<ItemCondition>(),
                           Arg.Any<string>(),
                           Arg.Any<CancellationToken>()
                       )
                       .Returns(new List<Item>());

        _mapper.Map<List<ItemDto>>(Arg.Any<List<Item>>())
               .Returns(new List<ItemDto>());

        // Act
        var actual = await _handler.Handle(searchItemsQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeNullOrEmpty();

        await _itemRepository.Received(1)
                             .SearchAsync(
                                 string.Empty,
                                 string.Empty,
                                 Arg.Any<ItemCondition>(),
                                 string.Empty,
                                 CancellationToken.None
                             );
    }

    [Fact]
    public async Task HandleSearchItemsQueryHandler_ShouldReturnItemsList_WhenItemsExist()
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

        var searchItemsQuery = new SearchItemsQuery(
            "item",
            string.Empty,
            string.Empty,
            string.Empty
        );

        _itemRepository
            .SearchAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ItemCondition>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(expectedItems);

        _mapper
            .Map<List<ItemDto>>(Arg.Any<List<Item>>())
            .Returns(expectedItemDtos);

        // Act
        var actual = await _handler.Handle(searchItemsQuery, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expectedItemDtos);

        await _itemRepository.Received(1)
                             .SearchAsync(
                                 searchItemsQuery.Title,
                                 string.Empty,
                                 Arg.Any<ItemCondition>(),
                                 string.Empty,
                                 CancellationToken.None
                             );
    }
}
