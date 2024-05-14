namespace FreeStuff.Identity.Api.Domain.Ports;

public interface ITokenRepository
{
    Task CreateAsync(Token token, CancellationToken cancellationToken);

    Task<Token?> GetAsync(Guid id, CancellationToken cancellationToken);

    void Delete(Token token);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
