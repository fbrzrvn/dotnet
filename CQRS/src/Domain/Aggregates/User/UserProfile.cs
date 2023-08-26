namespace Domain.Aggregates.User;

public sealed class UserProfile
{
    private UserProfile()
    {
    }

    public Guid UserProfileId { get; }

    public string IdentityId { get; private set; }

    public UserInfo UserInfo { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static UserProfile CreateUserProfile(string identityId, UserInfo userInfo)
    {
        UserProfile userProfile = new()
        {
            IdentityId = identityId, UserInfo = userInfo, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };

        return userProfile;
    }

    public void UpdateUserInfo(UserInfo newUserInfo)
    {
        UserInfo = newUserInfo;
        UpdatedAt = DateTime.UtcNow;
    }
}