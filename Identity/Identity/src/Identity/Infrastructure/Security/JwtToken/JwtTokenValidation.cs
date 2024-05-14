namespace Identity.Infrastructure.Security.JwtToken;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public sealed class JwtTokenValidation : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtTokenConfigurations _jwtTokenConfigurations;

    public JwtTokenValidation(IOptions<JwtTokenConfigurations> tokenConfigurations)
    {
        _jwtTokenConfigurations = tokenConfigurations.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = _jwtTokenConfigurations.Issuer,
            ValidAudience            = _jwtTokenConfigurations.Audience,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfigurations.Secret))
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
