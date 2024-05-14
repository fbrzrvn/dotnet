namespace Identity.Application.Commands.RefreshToken;

using Common.DTOs;
using ErrorOr;
using MediatR;
using Shared.Application.Security;

[Authorize]
public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ErrorOr<AccessCredentialsDto>>;
