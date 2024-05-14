using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;
using FreeStuff.Items.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Search;

public sealed class SearchItemsQueryHandler : IRequestHandler<SearchItemsQuery, ErrorOr<List<ItemDto>>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public SearchItemsQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<List<ItemDto>>> Handle(SearchItemsQuery request, CancellationToken cancellationToken)
    {
        var itemCondition = string.IsNullOrEmpty(request.Condition)
            ? ItemCondition.None
            : request.Condition.MapPartialStringToItemCondition();

        var items = await _itemRepository.SearchAsync(
            request.Title,
            request.CategoryName,
            itemCondition,
            request.SortBy,
            cancellationToken
        );

        var result = _mapper.Map<List<ItemDto>>((items ?? Array.Empty<Item>()).ToList());

        return result;
    }
}
