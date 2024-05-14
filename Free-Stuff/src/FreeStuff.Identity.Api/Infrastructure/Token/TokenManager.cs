using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreeStuff.Contracts.Identity.Responses;
using FreeStuff.Identity.Api.Application.Token;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Enum;
using FreeStuff.Identity.Api.Domain.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FreeStuff.Identity.Api.Infrastructure.Token;

public class TokenManager : ITokenManager
{
    private readonly TokenConfig      _tokenConfig;
    private readonly ITokenRepository _tokenRepository;

    public TokenManager(IOptions<TokenConfig> tokenConfig, ITokenRepository tokenRepository)
    {
        _tokenConfig     = tokenConfig.Value;
        _tokenRepository = tokenRepository;
    }

    public AuthenticationResponse GenerateTokens(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var accessTokenDescriptor = new JwtSecurityToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_tokenConfig.AccessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer             = _tokenConfig.Issuer,
            Audience           = _tokenConfig.Audience,
            Subject            = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }),
            Expires            = DateTime.Now.AddDays(_tokenConfig.RefreshTokenExpiryDays),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken  = tokenHandler.WriteToken(accessTokenDescriptor);
        var token        = tokenHandler.CreateToken(refreshTokenDescriptor);
        var refreshToken = tokenHandler.WriteToken(token);

        return new AuthenticationResponse(accessToken, refreshToken);
    }

    public async Task<Domain.Token> GenerateTokenAsync(User user, CancellationToken cancellationToken)
    {
        var token = Domain.Token.Create(user.Id, TokenType.RefreshToken);

        await _tokenRepository.CreateAsync(token, cancellationToken);
        await _tokenRepository.SaveChangesAsync(cancellationToken);

        return token;
    }
}
