using Domain.Aggregates.User;
using MediatR;

namespace Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; init; }
}