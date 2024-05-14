namespace Identity.Application.Commands.ResetPassword;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

public record ResetPasswordCommand(string Email, string Token, string Password) : IRequest<ErrorOr<IdentityResult>>;
