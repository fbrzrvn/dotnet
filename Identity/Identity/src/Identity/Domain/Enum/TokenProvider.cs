namespace Identity.Domain.Enum;

using Ardalis.SmartEnum;

public class TokenProvider : SmartEnum<TokenProvider>
{
    public static readonly TokenProvider Chicly = new("Chicly", 1);

    private TokenProvider(string name, int value) : base(name, value)
    {
    }
}
