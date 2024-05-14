namespace Identity.Application.Commands.ResetPassword;

using Domain.Enum;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<IdentityResult>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<IdentityResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.Error("Failed to found user with email {@UserEmail}", request.Email);

            return Error.NotFound(description: "User not found");
        }

        await VerifyTokenAsync(user, request.Token);

        var result = await ResetPasswordForUserAsync(user, request.Token, request.Password);

        return ErrorOrFactory.From(result);
    }

    private async Task<ErrorOr<Success>> VerifyTokenAsync(IdentityUser user, string token)
    {
        var isTokenVerified = await _userManager.VerifyUserTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.PasswordReset.Name,
            token
        );

        if (isTokenVerified == false)
        {
            _logger.Error("Failed to verify reset password token for user with email {@UserEmail}", user.Email);

            return Error.Failure(description: "Access Denied: You do not have permission to access this resource");
        }

        return Result.Success;
    }

    private async Task<IdentityResult> ResetPasswordForUserAsync(IdentityUser user, string token, string password)
    {
        var result = await _userManager.ResetPasswordAsync(user, token, password);

        await _userManager.RemoveAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.PasswordReset.Name
        );

        _logger.Information("Successfully reset password user {@Username}", user.UserName);

        return result;
    }
}
