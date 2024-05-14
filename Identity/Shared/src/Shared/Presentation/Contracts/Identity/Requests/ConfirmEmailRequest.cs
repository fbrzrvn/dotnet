namespace Shared.Presentation.Contracts.Identity.Requests;

public record ConfirmEmailRequest(string Email, string Token);
