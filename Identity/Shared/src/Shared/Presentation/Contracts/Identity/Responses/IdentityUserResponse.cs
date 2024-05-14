namespace Shared.Presentation.Contracts.Identity.Responses;

public record IdentityUserResponse(
    string  Id,
    string  UserName,
    string  Email,
    string? PhoneNumber,
    string  Role
);
