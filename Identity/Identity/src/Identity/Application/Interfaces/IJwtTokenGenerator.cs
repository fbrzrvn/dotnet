namespace Identity.Application.Interfaces;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(IdentityUser user, IEnumerable<string> roles);

    ClaimsPrincipal GetClaimsFromExpiredToken(string token);
}
