namespace Shared.Presentation.Contracts.Identity.Requests;

public record RegisterUserRequest(
    string  UserName,
    string  Email,
    string  Password,
    string? PhoneNumber
);
