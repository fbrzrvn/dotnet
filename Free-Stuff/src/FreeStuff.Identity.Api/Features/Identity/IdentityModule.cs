using Carter;
using FreeStuff.Contracts.Identity.Requests;
using FreeStuff.Identity.Api.Application.Services;

namespace FreeStuff.Identity.Api.Features.Identity;

public class IdentityModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Register, RegisterUserAsync);
        app.MapPost(ApiEndpoints.Login, AuthenticateAsync);
        app.MapPost(ApiEndpoints.RefreshToken, RefreshTokenAsync).RequireAuthorization();
        app.MapPost(ApiEndpoints.AdminOnly, AdminOnly).RequireAuthorization();
        app.MapPost(ApiEndpoints.ForgotPassword, ForgotPasswordAsync);
    }

    private static async Task<IResult> RegisterUserAsync(RegisterUserRequest request, IUserService userService)
    {
        return await userService.RegisterUserAsync(request);
    }

    private static async Task<IResult> AuthenticateAsync(
        AuthenticationRequest  request,
        IAuthenticationService authenticationService
    )
    {
        return await authenticationService.AuthenticateAsync(request);
    }

    private static async Task<IResult> RefreshTokenAsync(
        RefreshTokenRequest    request,
        IHttpContextAccessor   contextAccessor,
        IAuthenticationService authenticationService
    )
    {
        var claims = contextAccessor.HttpContext!.User;

        return await authenticationService.RefreshTokenAsync(request, claims);
    }

    private static async Task<IResult> AdminOnly(IHttpContextAccessor contextAccessor, IUserService userService)
    {
        var claims = contextAccessor.HttpContext!.User;

        return await userService.AdminOnly(claims);
    }

    private static async Task<IResult> ForgotPasswordAsync(
        ForgotPasswordRequest request,
        IUserService          userService,
        CancellationToken     cancellationToken
    )
    {
        return await userService.ForgotPasswordAsync(request, cancellationToken);
    }
}
