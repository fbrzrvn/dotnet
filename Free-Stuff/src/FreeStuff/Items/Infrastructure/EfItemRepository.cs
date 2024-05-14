using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Enum;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FreeStuff.Items.Infrastructure;

public class EfItemRepository : IItemRepository
{
    private readonly FreeStuffDbContext _context;

    public EfItemRepository(FreeStuffDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Item item, CancellationToken cancellationToken)
    {
        await _context.AddAsync(item, cancellationToken);
    }

    public async Task<Item?> GetAsync(ItemId id, CancellationToken cancellationToken)
    {
        var item = await _context.Items.Include(item => item.Category)
                                 .SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

        return item;
    }

    public async Task<IEnumerable<Item>?> GetAllAsync(int page, int limit, CancellationToken cancellationToken)
    {
        var items = await _context.Items.Include(item => item.Category)
                                  .OrderByDescending(item => item.CreatedDateTime)
                                  .Skip((page - 1) * limit)
                                  .Take(limit)
                                  .ToListAsync(cancellationToken);

        return items;
    }

    public async Task<IEnumerable<Item>?> SearchAsync(
        string?           title,
        string?           categoryName,
        ItemCondition?    condition,
        string?           sortBy,
        CancellationToken cancellationToken
    )
    {
        var query = _context.Items.Include(item => item.Category).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(item => item.Title.ToLower().Contains(title.ToLower()));
        }

        if (!string.IsNullOrEmpty(categoryName))
        {
            query = query.Where(item => item.Category.Name.ToLower().Contains(categoryName.ToLower()));
        }

        if (condition != ItemCondition.None)
        {
            query = query.Where(item => item.Condition == condition);
        }

        query = sortBy switch
        {
            "asc"  => query.OrderBy(item => item.Title),
            "desc" => query.OrderByDescending(item => item.Title),
            _      => query
        };

        return await query.ToListAsync(cancellationToken);
    }

    public void Update(Item item)
    {
        _context.Items.Update(item);
    }

    public void Delete(Item item)
    {
        _context.Items.Remove(item);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public int CountItems()
    {
        return _context.Items.Count();
    }
}
