namespace Identity.Application.Commands.Update;

using Domain.Enum;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

public class UpdateIdentityUserCommandHandler : IRequestHandler<UpdateIdentityUserCommand, ErrorOr<Success>>
{
    private readonly ILogger                   _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public UpdateIdentityUserCommandHandler(UserManager<IdentityUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger      = logger;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateIdentityUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            _logger.Error("Failed to found user with ID {@UserId}", request.UserId);

            return Error.NotFound(description: "User not found");
        }

        if (string.IsNullOrEmpty(request.Role) == false)
        {
            await UpdateUserRoles(user, request.Role);
        }

        return await UpdateUserAsync(request, user);
    }

    private async Task<ErrorOr<Success>> UpdateUserRoles(IdentityUser user, string newRole)
    {
        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains(newRole))
        {
            return Result.Success;
        }

        if (await _userManager.IsInRoleAsync(user, Role.Admin.Name) == false)
        {
            return Error.Failure(description: "Only admins can update user roles");
        }

        var removeResult = await _userManager.RemoveFromRolesAsync(user, roles);

        if (removeResult.Succeeded == false)
        {
            return Error.Failure(description: removeResult.Errors.First().Description);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, newRole);

        if (roleResult.Succeeded == false)
        {
            _logger.Error(
                "Failed to update user roles for role {@Role}. Reasons: {@Errors}",
                newRole,
                roleResult.Errors
            );

            return Error.Failure(description: $"Failed to update user roles for role {newRole}");
        }

        return Result.Success;
    }

    private async Task<ErrorOr<Success>> UpdateUserAsync(UpdateIdentityUserCommand request, IdentityUser user)
    {
        user.UserName    = request.UserName ?? user.UserName;
        user.Email       = request.Email ?? user.Email;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded == false)
        {
            _logger.Error("Failed to update user: {@Errors}", result.Errors);

            return Error.Unexpected(description: "Failed to update user");
        }

        _logger.Information("Successfully updated user {@Username}", request.UserName);

        return Result.Success;
    }
}
