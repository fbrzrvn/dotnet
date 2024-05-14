using FreeStuff.Identity.Api.Domain.Ports;
using FreeStuff.Identity.Api.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Identity.Api.Infrastructure.Token;

public class EfTokenRepository : ITokenRepository
{
    private readonly FreeStuffIdentityDbContext _context;

    public EfTokenRepository(FreeStuffIdentityDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Domain.Token token, CancellationToken cancellationToken)
    {
        await _context.Tokens.AddAsync(token, cancellationToken);
    }

    public async Task<Domain.Token?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var token = await _context.Tokens.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        return token;
    }

    public void Delete(Domain.Token token)
    {
        _context.Tokens.Remove(token);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
