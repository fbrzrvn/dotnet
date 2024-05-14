using ErrorOr;
using FreeStuff.Contracts.Items.Events;
using FreeStuff.Items.Domain.Errors;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Domain;
using MediatR;

namespace FreeStuff.Items.Application.Delete;

public sealed class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ErrorOr<bool>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IEventBus       _eventBus;

    public DeleteItemCommandHandler(IItemRepository itemRepository, IEventBus eventBus)
    {
        _itemRepository = itemRepository;
        _eventBus       = eventBus;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetAsync(ItemId.Create(request.Id), cancellationToken);

        if (item is null)
        {
            return Errors.Item.NotFound(request.Id);
        }

        _itemRepository.Delete(item);

        await _itemRepository.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(new ItemDeleted(request.Id), cancellationToken);

        return true;
    }
}
