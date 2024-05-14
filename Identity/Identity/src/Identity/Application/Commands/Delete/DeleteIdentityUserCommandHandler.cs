namespace Identity.Application.Commands.Delete;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class DeleteIdentityUserCommandHandler : IRequestHandler<DeleteIdentityUserCommand, ErrorOr<IdentityResult>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public DeleteIdentityUserCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<IdentityResult>> Handle(
        DeleteIdentityUserCommand request,
        CancellationToken         cancellationToken
    )
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            _logger.Error("Failed to found user with ID {@UserId}", request.UserId);

            return Error.NotFound(description: "User not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded == false)
        {
            _logger.Error("Failed to delete user {@Username}. Reasons: {@Errors}", user.UserName, result.Errors);
        }

        _logger.Information("Successfully deleted user {@Username} at {@DeletionTime}", user.UserName, DateTime.UtcNow);

        return ErrorOrFactory.From(result);
    }
}
