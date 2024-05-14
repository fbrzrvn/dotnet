namespace Identity.Application.Commands.Logout;

using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

public record LogoutUserCommand(ClaimsPrincipal Claims) : IRequest<ErrorOr<IdentityResult>>;
