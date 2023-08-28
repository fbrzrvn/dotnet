namespace WebApi.Contracts.UserProfiles.Responses;

public record UserProfileResponse
{
    public Guid UserProfileId { get; set; }

    public UserInfoResponse UserInfo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}