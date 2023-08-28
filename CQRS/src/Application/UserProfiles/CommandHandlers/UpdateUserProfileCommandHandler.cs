using Application.UserProfiles.Commands;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        UserProfile? userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(userProfile =>
            userProfile.UserProfileId == request.UserProfileId, cancellationToken);

        UserInfo userInfo = UserInfo.CreateUserInfo(request.FirstName, request.LastName, request.EmailAddress,
            request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

        userProfile!.UpdateUserInfo(userInfo);

        _ctx.UserProfiles.Update(userProfile);
        await _ctx.SaveChangesAsync(cancellationToken);
    }
}