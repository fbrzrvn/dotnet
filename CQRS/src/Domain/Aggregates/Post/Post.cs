using Domain.Aggregates.User;

namespace Domain.Aggregates.Post;

public sealed class Post
{
    private readonly List<PostComment> _comments = new();
    private readonly List<PostInteraction> _interactions = new();

    private Post()
    {
    }

    public Guid PostId { get; set; }

    public Guid UserProfileId { get; private set; }

    public UserProfile UserProfile { get; }

    public string Text { get; private set; }

    public IEnumerable<PostComment> Comments => _comments;

    public IEnumerable<PostInteraction> Interactions => _interactions;

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Post CreatePost(Guid userProfileId, string text)
    {
        Post post = new()
        {
            UserProfileId = userProfileId, Text = text, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        return post;
    }

    public void UpdatePost(string text)
    {
        Text = text;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddCommentToPost(PostComment comment)
    {
        _comments.Add(comment);
    }

    public void RemoveCommentFromPost(PostComment comment)
    {
        _comments.Remove(comment);
    }

    public void AddInteractionToPost(PostInteraction interaction)
    {
        _interactions.Add(interaction);
    }

    public void RemoveInteractionFromPost(PostInteraction interaction)
    {
        _interactions.Remove(interaction);
    }
}