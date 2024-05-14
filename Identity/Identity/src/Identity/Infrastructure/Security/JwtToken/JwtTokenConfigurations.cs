namespace Identity.Infrastructure.Security.JwtToken;

public sealed class JwtTokenConfigurations
{
    public const string Section = "JwtToken";

    public string Issuer                  { get; init; } = string.Empty;
    public string Audience                { get; init; } = string.Empty;
    public string Secret                  { get; init; } = string.Empty;
    public int    ExpirationTimeInMinutes { get; init; }
}
