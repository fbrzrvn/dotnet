using FreeStuff.Shared.Domain;

namespace FreeStuff.Items.Domain.ValueObjects;

public sealed class ItemId : ValueObject
{
    public Guid Value { get; }

    private ItemId(Guid value)
    {
        Value = value;
    }

    public ItemId()
    {
    }

    public static ItemId CreateUnique()
    {
        return new ItemId(Guid.NewGuid());
    }

    public static ItemId Create(Guid id)
    {
        return new ItemId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
