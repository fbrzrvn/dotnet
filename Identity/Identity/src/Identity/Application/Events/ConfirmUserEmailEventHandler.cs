// namespace Identity.Application.Events;
//
// using Domain.Enum;
// using Domain.Events;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
// using Serilog;
//
// public class ConfirmUserEmailEventHandler : INotificationHandler<UserCreatedEvent>
// {
//     private readonly ILogger                   _logger;
//     private readonly UserManager<IdentityUser> _userManager;
//
//     public ConfirmUserEmailEventHandler(UserManager<IdentityUser> userManager, ILogger logger)
//     {
//         _userManager = userManager;
//         _logger      = logger;
//     }
//
//     public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
//     {
//         var user = await _userManager.FindByIdAsync(notification.UserId);
//
//         if (user == null)
//         {
//             _logger.Warning($"User with ID '{notification.UserId}' not found. Skipping email confirmation.");
//
//             return;
//         }
//
//         var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//
//         await _userManager.SetAuthenticationTokenAsync(
//             user,
//             TokenProvider.Chicly.Name,
//             TokenType.EmailConfirmation.Name,
//             emailToken
//         );
//
//         _logger.Debug("Successfully generated token EmailConfirmationToken: {@Token}", emailToken);
//
//         // ToDo: Implement email service for sending confirmation email
//
//         _logger.Information(
//             "Sending email confirmation to user with email: {@UserEmail}. Token to confirm email: {@Token}",
//             user.Email,
//             emailToken
//         );
//     }
// }



