namespace Identity.Application.Commands.ChangePassword;

using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Security;

[Authorize]
public record ChangePasswordCommand(ClaimsPrincipal Claims, string CurrentPassword, string NewPassword)
    : IRequest<ErrorOr<IdentityResult>>;
