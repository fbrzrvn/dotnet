using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Categories.Infrastructure;

public class EfCategoryRepository : ICategoryRepository
{
    private readonly FreeStuffDbContext _context;

    public EfCategoryRepository(FreeStuffDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.AddAsync(category, cancellationToken);
    }

    public async Task<Category?> GetAsync(string categoryName, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.SingleOrDefaultAsync(
            category => category.Name == categoryName,
            cancellationToken
        );

        return category;
    }

    public async Task<IEnumerable<Category>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
                                       .OrderByDescending(category => category.Name)
                                       .ToListAsync(cancellationToken);

        return categories;
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
