using Domain.Aggregates.User;
using MediatR;

namespace Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; init; }
}