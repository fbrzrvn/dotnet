using FreeStuff.Contracts.Identity.Responses;
using FreeStuff.Identity.Api.Domain;

namespace FreeStuff.Identity.Api.Application.Token;

public interface ITokenManager
{
    AuthenticationResponse GenerateTokens(User user, IEnumerable<string> roles);

    Task<Domain.Token> GenerateTokenAsync(User user, CancellationToken cancellationToken);
}
