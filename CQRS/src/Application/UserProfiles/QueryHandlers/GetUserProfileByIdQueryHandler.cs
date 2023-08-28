using Application.UserProfiles.Queries;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfile>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<UserProfile> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        UserProfile? userProfile =
            await _ctx.UserProfiles.FirstOrDefaultAsync(
                userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

        return userProfile!;
    }
}