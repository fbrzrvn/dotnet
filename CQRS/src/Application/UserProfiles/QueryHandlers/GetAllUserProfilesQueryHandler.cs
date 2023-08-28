using Application.UserProfiles.Queries;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, IEnumerable<UserProfile>>
{
    private readonly DataContext _ctx;

    public GetAllUserProfilesQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<UserProfile>> Handle(GetAllUserProfilesQuery request, CancellationToken
        cancellationToken)
    {
        List<UserProfile> userProfiles = await _ctx.UserProfiles.ToListAsync(cancellationToken);

        return userProfiles;
    }
}