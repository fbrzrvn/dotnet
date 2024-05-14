using System.Security.Claims;
using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Application.Services;
using FreeStuff.Identity.Api.Application.Token;
using FreeStuff.Identity.Api.Domain;
using Microsoft.AspNetCore.Identity;

namespace FreeStuff.Identity.Api.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private const string LocalTokenProvider  = "Local";
    private const string RefreshTokenPurpose = "RefreshToken";

    private readonly ITokenManager     _tokenManager;
    private readonly UserManager<User> _userManager;

    public AuthenticationService(ITokenManager tokenManager, UserManager<User> userManager)
    {
        _tokenManager = tokenManager;
        _userManager  = userManager;
    }

    public async Task<IResult> AuthenticateAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.Unauthorized();
        }

        var roles  = await _userManager.GetRolesAsync(user);
        var tokens = _tokenManager.GenerateTokens(user, roles);

        await _userManager.SetAuthenticationTokenAsync(
            user,
            LocalTokenProvider,
            RefreshTokenPurpose,
            tokens.RefreshToken
        );

        return Results.Ok(tokens);
    }

    public async Task<IResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUserAsync(claims);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        var existingToken =
            await _userManager.GetAuthenticationTokenAsync(user, LocalTokenProvider, RefreshTokenPurpose);

        if (existingToken == null || existingToken != refreshTokenRequest.Token)
        {
            return Results.Unauthorized();
        }

        var roles  = await _userManager.GetRolesAsync(user);
        var tokens = _tokenManager.GenerateTokens(user, roles);

        await _userManager.RemoveAuthenticationTokenAsync(user, LocalTokenProvider, RefreshTokenPurpose);
        await _userManager.SetAuthenticationTokenAsync(
            user,
            LocalTokenProvider,
            RefreshTokenPurpose,
            tokens.RefreshToken
        );

        return Results.Ok(tokens);
    }
}
