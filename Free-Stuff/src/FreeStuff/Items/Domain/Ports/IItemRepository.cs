using FreeStuff.Items.Domain.Enum;
using FreeStuff.Items.Domain.ValueObjects;

namespace FreeStuff.Items.Domain.Ports;

public interface IItemRepository
{
    Task CreateAsync(Item item, CancellationToken cancellationToken);

    Task<Item?> GetAsync(ItemId id, CancellationToken cancellationToken);

    Task<IEnumerable<Item>?> GetAllAsync(int page, int limit, CancellationToken cancellationToken);

    Task<IEnumerable<Item>?> SearchAsync(
        string?           title,
        string?           categoryName,
        ItemCondition?    condition,
        string?           sortBy,
        CancellationToken cancellationToken
    );

    void Update(Item item);

    void Delete(Item item);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    int CountItems();
}
