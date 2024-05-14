namespace Shared.Infrastructure.Security.CurrentUserProvider;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _httpContextAccessor.HttpContext!.Response.StatusCode = 401;

            return null!;
        }

        var id    = Guid.Parse(GetSingleClaimValue(ClaimTypes.NameIdentifier));
        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(id, roles);
    }

    private List<string> GetClaimValues(string claimType)
    {
        return _httpContextAccessor.HttpContext!
            .User
            .Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }

    private string GetSingleClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext!.User.Claims.Single(claim => claim.Type == claimType).Value;
    }
}
