namespace FreeStuff.Contracts.Identity.Responses;

public record AuthenticationResponse(string AccessToken, string RefreshToken);
