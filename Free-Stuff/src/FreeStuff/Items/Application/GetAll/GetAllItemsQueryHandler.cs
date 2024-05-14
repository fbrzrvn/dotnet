using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.GetAll;

public sealed class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, ErrorOr<ItemsDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public GetAllItemsQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<ItemsDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAllAsync(
                        request.Page,
                        request.Limit,
                        cancellationToken
                    ) ??
                    new List<Item>();

        var totalItems = _itemRepository.CountItems();

        var result = new ItemsDto(_mapper.Map<List<ItemDto>>(items), totalItems);

        return result;
    }
}
