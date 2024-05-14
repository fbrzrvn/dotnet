namespace Identity.Application.Commands.ForgotPassword;

using ErrorOr;
using MediatR;

public record ForgotPasswordCommand(string Email) : IRequest<ErrorOr<string>>;
