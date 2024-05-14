namespace FreeStuff.Identity.Api.Application.Token;

public sealed class TokenConfig
{
    public string Issuer                            { get; set; } = string.Empty;
    public string Audience                          { get; set; } = string.Empty;
    public string Secret                            { get; set; } = string.Empty;
    public int    AccessTokenExpiryMinutes          { get; set; }
    public int    RefreshTokenExpiryDays            { get; set; }
    public int    ResetPasswordTokenExpiryMinutes   { get; set; }
    public int    EmailConfirmationTokenExpiryHours { get; set; }
}
