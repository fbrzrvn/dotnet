namespace FreeStuff.Categories.Application.Shared.Dto;

public class CategoryDto
{
    public Guid   Id          { get; }
    public string Name        { get; }
    public string Description { get; }

    public CategoryDto(Guid id, string name, string description)
    {
        Id          = id;
        Name        = name;
        Description = description;
    }
}
