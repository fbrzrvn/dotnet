namespace Identity.Application.Commands.Logout;

using Domain.Enum;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, ErrorOr<IdentityResult>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public LogoutUserCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<IdentityResult>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(request.Claims);

        if (user == null)
        {
            _logger.Error(
                "Failed to found user with claims {@Claims}. Username was {@Username}",
                request.Claims,
                request.Claims.Identity?.Name
            );

            return Error.NotFound(description: "User not found");
        }

        var result = await _userManager.RemoveAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.Refresh.Name
        );

        if (result.Succeeded == false)
        {
            _logger.Error("Failed to log out user {@Username}. Reasons: {@Errors}", user.UserName, result.Errors);
        }

        _logger.Information("Successfully logged out user {@Username}", user.UserName);

        return ErrorOrFactory.From(result);
    }
}
