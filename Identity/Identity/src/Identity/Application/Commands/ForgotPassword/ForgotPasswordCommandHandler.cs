namespace Identity.Application.Commands.ForgotPassword;

using Domain.Enum;
using Domain.Events;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ErrorOr<string>>
{
    private readonly IPublisher                _eventBus;
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ForgotPasswordCommandHandler(UserManager<IdentityUser> userManager, IPublisher eventBus, ILogger logger)
    {
        _userManager = userManager;
        _eventBus    = eventBus;
        _logger      = logger;
    }

    public async Task<ErrorOr<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.Information("Password reset requested for non-existent user with email {@Email}", request.Email);

            return ErrorOrFactory.From(
                $"Attempted password reset for user with email {request.Email}, but user not found"
            );
        }

        var resetToken = await GenerateAndSetResetTokenAsync(user);

        var passwordResetRequestedEvent = new PasswordResetRequestedEvent(user, resetToken);
        await _eventBus.Publish(passwordResetRequestedEvent, cancellationToken);

        return ErrorOrFactory.From(resetToken);
    }

    private async Task<string> GenerateAndSetResetTokenAsync(IdentityUser user)
    {
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _userManager.SetAuthenticationTokenAsync(
            user,
            TokenProvider.Chicly.Name,
            TokenType.PasswordReset.Name,
            resetToken
        );

        _logger.Information(
            "Successfully generated password reset token for user {@Username} with email {@Email}.",
            user.UserName,
            user.Email
        );

        return resetToken;
    }
}
