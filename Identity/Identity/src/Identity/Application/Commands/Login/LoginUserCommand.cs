namespace Identity.Application.Commands.Login;

using Common.DTOs;
using ErrorOr;
using MediatR;

public record LoginUserCommand(string UserName, string Password) : IRequest<ErrorOr<AccessCredentialsDto>>;
