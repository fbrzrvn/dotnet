namespace Identity.Application.Commands.ConfirmEmail;

using Domain.Enum;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<IdentityResult>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<IdentityResult>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.Error("Failed to found user with email {@UserEmail}", request.Email);

            return Error.NotFound(description: "User not found");
        }

        var tokenProvider = TokenProvider.Chicly.Name;
        var tokenType     = TokenType.EmailConfirmation.Name;

        var existingToken = await _userManager.GetAuthenticationTokenAsync(user, tokenProvider, tokenType);

        if (existingToken == null)
        {
            _logger.Error("Failed to found email confirmation token for user with email {@UserEmail}", request.Email);

            return Error.Failure(description: "Access Denied: You do not have permission to access this resource");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded == false)
        {
            var confirmEmailErrors = string.Join(", ", result.Errors.Select(error => error.Description));
            _logger.Error(
                "Failed to confirm email for user with email {@UserEmail}. Reasons: {@Errors}",
                request.Email,
                confirmEmailErrors
            );

            return Error.Unexpected(description: $"User email confirmation failed. Reasons: {confirmEmailErrors}");
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, tokenProvider, tokenType);

        _logger.Information("Successfully confirmed email for user with email {@UserEmail}", request.Email);

        return ErrorOrFactory.From(result);
    }
}
