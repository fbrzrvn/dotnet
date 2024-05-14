namespace Identity.Application.Commands.Delete;

using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Security;
using Shared.Application.Security.Constants;

[Authorize(Policies = Policy.SelfOrAdmin)]
public record DeleteIdentityUserCommand(string UserId) : IAuthorizableRequest<ErrorOr<IdentityResult>>;
