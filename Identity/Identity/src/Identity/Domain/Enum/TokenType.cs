namespace Identity.Domain.Enum;

using Ardalis.SmartEnum;

public class TokenType : SmartEnum<TokenType>
{
    public static readonly TokenType Refresh           = new("Refresh", 1);
    public static readonly TokenType EmailConfirmation = new("EmailConfirmation", 2);
    public static readonly TokenType PasswordReset     = new("PasswordReset", 3);

    private TokenType(string name, int value) : base(name, value)
    {
    }
}
