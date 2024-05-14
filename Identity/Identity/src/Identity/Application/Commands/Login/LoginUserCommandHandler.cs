namespace Identity.Application.Commands.Login;

using Common.DTOs;
using Domain.Enum;
using ErrorOr;
using Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<AccessCredentialsDto>>
{
    private readonly IJwtTokenGenerator        _jwtTokenGenerator;
    private readonly ILogger                   _logger;
    private readonly IMapper                   _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginUserCommandHandler(
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
        LoginUserCommand  request,
        CancellationToken cancellationToken
    )
    {
        var result = await ValidateAndGetUserAsync(request.UserName, request.Password);
        var user   = result.Value;
        var tokens = await GenerateAndPersistTokensAsync(user);

        var accessCredentialsDto = _mapper.Map<AccessCredentialsDto>(tokens);

        return ErrorOrFactory.From(accessCredentialsDto);
    }

    private async Task<ErrorOr<IdentityUser>> ValidateAndGetUserAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            _logger.Error("Failed login attempt for user {@Username}: Invalid username or password", username);

            return Error.Unauthorized(description: "Invalid username or password");
        }

        if (await _userManager.CheckPasswordAsync(user, password) == false)
        {
            _logger.Error("Failed login attempt for user {@Username}: Invalid username or password", username);

            return Error.Unauthorized(description: "Invalid username or password");
        }

        if (await _userManager.IsEmailConfirmedAsync(user) == false)
        {
            _logger.Error("Failed login attempt for user {@Username}: Email must be confirmed", username);

            return Error.Validation(description: "Email must be confirmed before login");
        }

        return ErrorOrFactory.From(user);
    }

    private async Task<(string AccessToken, string RefreshToken)> GenerateAndPersistTokensAsync(IdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles);

        var existingToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name
        );

        if (existingToken != null)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, TokenProvider.Chicly.Name, TokenType.Refresh.Name);
        }

        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name
        );

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
