namespace Identity.Application.Commands.Create;

using Common.DTOs;
using ErrorOr;
using Shared.Application.Security;
using Shared.Application.Security.Constants;

[Authorize(Policies = Policy.MemberOrAdmin)]
public record CreateIdentityUserCommand(
    string  UserId,
    string  UserName,
    string  Email,
    string  Password,
    string? PhoneNumber = default,
    string? Role        = default
) : IAuthorizableRequest<ErrorOr<IdentityUserDto>>;
