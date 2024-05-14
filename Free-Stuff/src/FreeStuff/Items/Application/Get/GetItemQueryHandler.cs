using ErrorOr;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Items.Application.Get;

public sealed class GetItemQueryHandler : IRequestHandler<GetItemQuery, ErrorOr<ItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper         _mapper;

    public GetItemQueryHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper         = mapper;
    }

    public async Task<ErrorOr<ItemDto>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id), cancellationToken);

        if (item is null)
        {
            return Errors.Item.NotFound(request.Id);
        }

        var result = _mapper.Map<ItemDto>(item);

        return result;
    }
}
