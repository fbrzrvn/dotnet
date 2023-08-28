using Application.UserProfiles.Commands;
using Domain.Aggregates.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        UserProfile? userProfile =
            await _ctx.UserProfiles.FirstOrDefaultAsync(
                userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

        _ctx.UserProfiles.Remove(userProfile!);
        await _ctx.SaveChangesAsync(cancellationToken);
    }
}