namespace Shared.Presentation.Contracts.Identity.Requests;

public record UpdateIdentityUserRequest(
    string? Email,
    string? UserName,
    string? PhoneNumber,
    string? Role
);
