namespace Domain.Aggregates.Post;

public sealed class PostInteraction
{
    private PostInteraction()
    {
    }

    public Guid InteractionId { get; }

    public Guid PostId { get; private set; }

    public InteractionType InteractionType { get; private set; }

    public static PostInteraction CreatePostInteraction(Guid postId, InteractionType interactionType)
    {
        PostInteraction postInteraction = new() { PostId = postId, InteractionType = interactionType };

        return postInteraction;
    }
}

public enum InteractionType
{
    Like,
    Dislike,
    Haha,
    Wow,
    Love,
    Angry
}