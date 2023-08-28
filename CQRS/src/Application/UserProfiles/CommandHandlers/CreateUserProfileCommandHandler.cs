using Application.UserProfiles.Commands;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;

namespace Application.UserProfiles.CommandHandlers;

internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfile>
{
    private readonly DataContext _ctx;

    public CreateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        UserInfo userInfo = UserInfo.CreateUserInfo(request.FirstName, request.LastName, request.EmailAddress,
            request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

        UserProfile userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), userInfo);

        _ctx.UserProfiles.Add(userProfile);

        await _ctx.SaveChangesAsync(cancellationToken);

        return userProfile;
    }
}