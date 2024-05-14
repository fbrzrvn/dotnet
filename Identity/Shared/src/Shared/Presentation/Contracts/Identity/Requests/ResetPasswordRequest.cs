namespace Shared.Presentation.Contracts.Identity.Requests;

public record ResetPasswordRequest(string Email, string Token, string Password);
