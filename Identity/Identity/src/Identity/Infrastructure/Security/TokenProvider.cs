namespace Identity.Infrastructure.Security;

using Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
    public TokenProvider(
        IDataProtectionProvider                      dataProtectionProvider,
        IOptions<DataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>>   logger
    ) : base(dataProtectionProvider, options, logger)
    {
    }

    public override Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var tokenLifespan = GetLifespanForPurpose(purpose);

        Options.TokenLifespan = tokenLifespan;

        return base.GenerateAsync(purpose, manager, user);
    }

    private TimeSpan GetLifespanForPurpose(string purpose)
    {
        return purpose switch
        {
            nameof(TokenType.EmailConfirmation) => TimeSpan.FromHours(24),
            nameof(TokenType.Refresh)           => TimeSpan.FromHours(12),
            nameof(TokenType.PasswordReset)     => TimeSpan.FromHours(1),
            _                                   => Options.TokenLifespan
        };
    }
}
