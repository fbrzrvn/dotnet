namespace FreeStuff.Contracts.Identity.Requests;

public record RegisterUserRequest(
    string  FirstName,
    string  LastName,
    string  UserName,
    string  Email,
    string  Password,
    string? Role
);
