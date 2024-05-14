using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.ValueObjects;
using FreeStuff.Items.Domain.Enum;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Shared.Domain;
using FreeStuff.User.Domain.ValueObjects;

namespace FreeStuff.Items.Domain;

public class Item : AggregateRoot<ItemId>
{
    public string        Title           { get; private set; }
    public string        Description     { get; private set; }
    public Category      Category        { get; private set; }
    public CategoryId    CategoryId      { get; private set; }
    public ItemCondition Condition       { get; private set; }
    public UserId        UserId          { get; private set; }
    public DateTime      CreatedDateTime { get; private set; }
    public DateTime      UpdatedDateTime { get; private set; }

    private Item(
        ItemId        id,
        string        title,
        string        description,
        Category      category,
        CategoryId    categoryId,
        ItemCondition condition,
        UserId        userId,
        DateTime      createdDateTime,
        DateTime      updatedDateTime
    ) : base(id)
    {
        Title           = title;
        Description     = description;
        Category        = category;
        CategoryId      = categoryId;
        Condition       = condition;
        UserId          = userId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private Item()
    {
    }

    public static Item Create(
        string        title,
        string        description,
        Category      category,
        ItemCondition condition,
        Guid          userId
    )
    {
        var item = new Item(
            ItemId.CreateUnique(),
            title,
            description,
            category,
            category.Id,
            condition,
            UserId.Create(userId),
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return item;
    }

    public void Update(
        string        title,
        string        description,
        Category      category,
        ItemCondition condition
    )
    {
        Title           = title;
        Description     = description;
        Category        = category;
        CategoryId      = category.Id;
        Condition       = condition;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
