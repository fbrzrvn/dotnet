namespace Identity.Application.Commands.ChangePassword;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<IdentityResult>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<IdentityResult>> Handle(
        ChangePasswordCommand request,
        CancellationToken     cancellationToken
    )
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

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded == false)
        {
            var changePasswordErrors = string.Join(", ", result.Errors.Select(error => error.Description));
            _logger.Error(
                "Failed to change password for user {@Username}. Reasons: {@Errors}",
                user.UserName,
                changePasswordErrors
            );

            return Error.Validation(
                description: $"Failed to change password for user {user.UserName}. Reasons: {changePasswordErrors}"
            );
        }

        _logger.Information("Successfully changed password for user {@Username}", user.UserName);

        return ErrorOrFactory.From(result);
    }
}
