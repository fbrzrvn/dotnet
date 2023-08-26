namespace Domain;

public sealed class Post
{
    public Guid Id { get; set; }

    public string Text { get; set; } = String.Empty;
}