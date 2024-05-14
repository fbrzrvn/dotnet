namespace FreeStuff.Contracts.Items.Responses;

public class ItemResponse
{
    public Guid   Id           { get; }
    public string Title        { get; }
    public string Description  { get; }
    public string CategoryName { get; }
    public string Condition    { get; }
    public Guid   UserId       { get; }

    public ItemResponse(
        Guid   id,
        string title,
        string description,
        string categoryName,
        string condition,
        Guid   userId
    )
    {
        Id           = id;
        Title        = title;
        Description  = description;
        CategoryName = categoryName;
        Condition    = condition;
        UserId       = userId;
    }
}
