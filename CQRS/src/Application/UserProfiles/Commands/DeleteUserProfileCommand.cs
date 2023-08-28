using MediatR;

namespace Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest
{
    public Guid UserProfileId { get; init; }
}