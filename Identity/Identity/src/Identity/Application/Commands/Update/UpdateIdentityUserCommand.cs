namespace Identity.Application.Commands.Update;

using ErrorOr;
using Shared.Application.Security;
using Shared.Application.Security.Constants;

[Authorize(Policies = Policy.SelfOrAdmin)]
public record UpdateIdentityUserCommand(
    string  UserId,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string? Role
) : IAuthorizableRequest<ErrorOr<Success>>;
