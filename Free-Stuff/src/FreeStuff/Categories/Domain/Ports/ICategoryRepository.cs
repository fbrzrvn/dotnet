namespace FreeStuff.Categories.Domain.Ports;

public interface ICategoryRepository
{
    Task CreateAsync(Category category, CancellationToken cancellationToken);

    Task<Category?> GetAsync(string categoryName, CancellationToken cancellationToken);

    Task<IEnumerable<Category>?> GetAllAsync(CancellationToken cancellationToken);

    void Update(Category category);

    void Delete(Category category);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
