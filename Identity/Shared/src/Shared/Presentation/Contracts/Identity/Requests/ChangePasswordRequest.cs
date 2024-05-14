namespace Shared.Presentation.Contracts.Identity.Requests;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
