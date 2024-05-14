namespace Identity.Application.Common.DTOs;

public record IdentityUserDto(
    string  Id,
    string  UserName,
    string  Email,
    string? PhoneNumber = default,
    string? Role        = default
);
