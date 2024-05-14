namespace Identity.Infrastructure.Security.JwtToken;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtTokenConfigurations _jwtTokenConfigurations;

    public JwtTokenGenerator(IOptions<JwtTokenConfigurations> tokenConfigurations)
    {
        _jwtTokenConfigurations = tokenConfigurations.Value;
    }

    public string GenerateAccessToken(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfigurations.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            _jwtTokenConfigurations.Issuer,
            _jwtTokenConfigurations.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtTokenConfigurations.ExpirationTimeInMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken  = tokenHandler.WriteToken(tokenDescriptor);

        return accessToken;
    }

    public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = _jwtTokenConfigurations.Issuer,
            ValidAudience            = _jwtTokenConfigurations.Audience,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfigurations.Secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal    = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Sha256) == false)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
