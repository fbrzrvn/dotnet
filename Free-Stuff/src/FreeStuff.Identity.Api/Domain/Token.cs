using System.Security.Cryptography;
using FreeStuff.Identity.Api.Domain.Enum;

namespace FreeStuff.Identity.Api.Domain;

public class Token
{
    public Guid     Id                 { get; private set; }
    public string   TokenType          { get; private set; } = string.Empty;
    public string   Value              { get; private set; } = string.Empty;
    public DateTime IssuedDateTime     { get; private set; }
    public DateTime ExpirationDateTime { get; private set; }
    public string   UserId             { get; private set; } = string.Empty;

    private Token(
        Guid     id,
        string   tokenType,
        string   value,
        DateTime issuedDateTime,
        DateTime expirationDateTime,
        string   userId
    )
    {
        Id                 = id;
        TokenType          = tokenType;
        Value              = value;
        IssuedDateTime     = issuedDateTime;
        ExpirationDateTime = expirationDateTime;
        UserId             = userId;
    }

    private Token()
    {
    }

    public static Token Create(string userId, TokenType tokenType, string? value = null)
    {
        var token = new Token(
            Guid.NewGuid(),
            tokenType.Name,
            value ?? GenerateToken(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(tokenType.ExpirationTime),
            userId
        );

        return token;
    }

    private static string GenerateToken()
    {
        using var rng = RandomNumberGenerator.Create();

        var randomBytes = new byte[32];
        rng.GetBytes(randomBytes);

        var token = Convert.ToBase64String(randomBytes);

        return token;
    }
}
