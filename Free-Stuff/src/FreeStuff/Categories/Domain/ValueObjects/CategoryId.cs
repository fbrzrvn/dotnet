using FreeStuff.Shared.Domain;

namespace FreeStuff.Categories.Domain.ValueObjects;

public class CategoryId : ValueObject
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public CategoryId()
    {
    }

    public static CategoryId CreateUnique()
    {
        return new CategoryId(Guid.NewGuid());
    }

    public static CategoryId Create(Guid id)
    {
        return new CategoryId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
