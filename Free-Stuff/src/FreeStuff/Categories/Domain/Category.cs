using FreeStuff.Categories.Domain.ValueObjects;
using FreeStuff.Items.Domain;
using FreeStuff.Shared.Domain;

namespace FreeStuff.Categories.Domain;

public class Category : Entity<CategoryId>
{
    public string     Name            { get; private set; }
    public string     Description     { get; private set; }
    public List<Item> Items           { get; private set; }
    public DateTime   CreatedDateTime { get; private set; }
    public DateTime   UpdatedDateTime { get; private set; }

    public Category(
        CategoryId id,
        string     name,
        string     description,
        DateTime   createdDateTime,
        DateTime   updatedDateTime
    ) : base(id)
    {
        Name            = name;
        Description     = description;
        Items           = new List<Item>();
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private Category()
    {
    }

    public static Category Create(string name)
    {
        var category = new Category(
            CategoryId.CreateUnique(),
            name,
            string.Empty,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return category;
    }

    public static Category Create(string name, string description)
    {
        var category = new Category(
            CategoryId.CreateUnique(),
            name,
            description,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return category;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    public void Update(string newName)
    {
        Name            = newName;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void Update(string newName, string description)
    {
        Name            = newName;
        Description     = description ?? Description;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
