namespace Identity.Domain;

using Shared.Domain;

public class IdentityUserId : ValueObject
{
    public Guid Value { get; }

    public IdentityUserId()
    {
    }

    private IdentityUserId(Guid value)
    {
        Value = value;
    }

    public static IdentityUserId CreateUnique()
    {
        return new IdentityUserId(Guid.NewGuid());
    }

    public static IdentityUserId Create(Guid id)
    {
        return new IdentityUserId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
