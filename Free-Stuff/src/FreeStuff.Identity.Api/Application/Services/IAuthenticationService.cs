using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;

namespace FreeStuff.Identity.Api.Application.Services;

public interface IAuthenticationService
{
    Task<IResult> AuthenticateAsync(AuthenticationRequest request);

    Task<IResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, ClaimsPrincipal claims);
}
