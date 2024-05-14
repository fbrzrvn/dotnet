namespace Shared.Presentation.Contracts.Identity.Requests;

public record CreateIdentityUserRequest(
    string  UserName,
    string  Email,
    string  Password,
    string? PhoneNumber,
    string? Role
);
