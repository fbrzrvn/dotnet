using Ardalis.SmartEnum;

namespace FreeStuff.Identity.Api.Domain.Enum;

public class TokenType : SmartEnum<TokenType>
{
    public static readonly TokenType RefreshToken      = new("RefreshToken", 1, 10080);
    public static readonly TokenType ResetPassword     = new("ResetPassword", 2, 15);
    public static readonly TokenType EmailConfirmation = new("EmailConfirmation", 3, 720);

    public readonly int ExpirationTime;


    public TokenType(string name, int value, int expirationTime) : base(name, value)
    {
        ExpirationTime = expirationTime;
    }
}
