namespace Domain.Aggregates.Post;

public sealed class PostComment
{
    private PostComment()
    {
    }

    public Guid CommentId { get; }

    public Guid PostId { get; private set; }

    public Guid UserProfileId { get; private set; }

    public string Text { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static PostComment CreatePostComment(Guid postId, Guid userProfileId, string text)
    {
        PostComment postComment = new()
        {
            PostId = postId,
            UserProfileId = userProfileId,
            Text = text,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return postComment;
    }

    public void UpdatePostComment(string text)
    {
        Text = text;
        UpdatedAt = DateTime.UtcNow;
    }
}