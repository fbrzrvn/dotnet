using Ardalis.SmartEnum;

namespace FreeStuff.Identity.Api.Domain.Enum;

public class Role : SmartEnum<Role>
{
    public static readonly Role Admin       = new("Admin", 1, "ADMIN");
    public static readonly Role Member      = new("Member", 2, "MEMBER");
    public static readonly Role RegularUser = new("RegularUser", 3, "REGULAR_USER");

    public readonly string NormalizedName;

    private Role(string name, int value, string normalizedName) : base(name, value)
    {
        NormalizedName = normalizedName;
    }
}
