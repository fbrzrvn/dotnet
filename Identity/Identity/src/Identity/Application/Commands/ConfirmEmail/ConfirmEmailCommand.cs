namespace Identity.Application.Commands.ConfirmEmail;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

public record ConfirmEmailCommand(string Email, string Token) : IRequest<ErrorOr<IdentityResult>>;
