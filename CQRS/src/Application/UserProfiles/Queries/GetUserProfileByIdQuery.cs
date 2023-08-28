using Domain.Aggregates.User;
using MediatR;

namespace Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<UserProfile>
{
    public Guid UserProfileId { get; init; }
}