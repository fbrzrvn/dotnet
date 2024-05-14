namespace Identity.Application.Events;

using Domain.Events;
using MediatR;
using Serilog;

public class SendResetPasswordEventHandler : INotificationHandler<PasswordResetRequestedEvent>
{
    private readonly ILogger _logger;

    public SendResetPasswordEventHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(PasswordResetRequestedEvent notification, CancellationToken cancellationToken)
    {
        // ToDo: Send an email to the user with instructions and the reset token

        _logger.Information(
            "Sending email confirmation to user with email: {@UserEmail}. Token to confirm email: {@Token}",
            notification.User.Email,
            notification.Token
        );

        await Task.CompletedTask;
    }
}
