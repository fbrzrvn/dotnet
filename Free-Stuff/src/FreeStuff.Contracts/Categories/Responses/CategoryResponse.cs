namespace FreeStuff.Contracts.Categories.Responses;

public class CategoryResponse
{
    public Guid   Id          { get; }
    public string Name        { get; }
    public string Description { get; }

    public CategoryResponse(Guid id, string name, string description)
    {
        Id          = id;
        Name        = name;
        Description = description;
    }
}
