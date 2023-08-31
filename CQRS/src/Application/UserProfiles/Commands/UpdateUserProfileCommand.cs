using Domain.Aggregates.User;
using MediatR;

namespace Application.UserProfiles.Commands;

public class UpdateUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string CurrentCity { get; set; }
}