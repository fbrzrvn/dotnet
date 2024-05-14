namespace Identity.Application.Commands.Register;

using Common.DTOs;
using ErrorOr;
using MediatR;

public record RegisterUserCommand(
    string  UserName,
    string  Email,
    string  Password,
    string? PhoneNumber
) : IRequest<ErrorOr<IdentityUserDto>>;
