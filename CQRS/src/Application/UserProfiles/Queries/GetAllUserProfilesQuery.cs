using Domain.Aggregates.User;
using MediatR;

namespace Application.UserProfiles.Queries;

public class GetAllUserProfilesQuery : IRequest<IEnumerable<UserProfile>>
{
}