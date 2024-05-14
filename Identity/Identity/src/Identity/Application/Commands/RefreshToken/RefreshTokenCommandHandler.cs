namespace Identity.Application.Commands.RefreshToken;

using Common.DTOs;
using Domain.Enum;
using ErrorOr;
using Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AccessCredentialsDto>>
{
    private readonly IJwtTokenGenerator        _jwtTokenGenerator;
    private readonly ILogger                   _logger;
    private readonly IMapper                   _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public RefreshTokenCommandHandler(
        UserManager<IdentityUser> userManager,
        IJwtTokenGenerator        jwtTokenGenerator,
        IMapper                   mapper,
        ILogger                   logger
    )
    {
        _userManager       = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper            = mapper;
        _logger            = logger;
    }

    public async Task<ErrorOr<AccessCredentialsDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken   cancellationToken
    )
    {
        var claims   = _jwtTokenGenerator.GetClaimsFromExpiredToken(request.AccessToken);
        var username = claims.Identity?.Name!;
        var user     = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            _logger.Error("User {@Username} not found", username);

            return Error.NotFound(description: "User not found");
        }

        await ValidateRefreshTokenAsync(user, request.RefreshToken);

        var tokens               = await GenerateAndPersistTokensAsync(user);
        var accessCredentialsDto = _mapper.Map<AccessCredentialsDto>(tokens);

        return ErrorOrFactory.From(accessCredentialsDto);
    }

    private async Task<ErrorOr<Success>> ValidateRefreshTokenAsync(IdentityUser user, string refreshToken)
    {
        var existingToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name
        );

        if (existingToken == null || existingToken != refreshToken)
        {
            _logger.Error("Invalid or expired refresh token for user with ID {@UserId}", user.Id);

            return Error.Failure(description: "Invalid or expired refresh token");
        }

        return Result.Success;
    }

    private async Task<(string AccessToken, string RefreshToken)> GenerateAndPersistTokensAsync(IdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);
        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name
        );

        await _userManager.RemoveAuthenticationTokenAsync(user, TokenProvider.Chicly.Name, TokenType.Refresh.Name);

        await _userManager.SetAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name,
            refreshToken
        );

        _logger.Information("Successfully logged in user {@Username}", user.UserName);
        _logger.Debug(
            "Successfully generated access token for user {@Username}: {@AccessToken}",
            user.UserName,
            accessToken
        );
        _logger.Debug(
            "Successfully generated refresh token for user {@Username}: {@RefreshToken}",
            user.UserName,
            refreshToken
        );

        return (accessToken, refreshToken);
    }
}
